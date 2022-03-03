using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension;

using Microsoft.EntityFrameworkCore;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;
using UserService.Core.Models;

namespace UserService.Data.EntityWorkers
{
    public class ContractProfileGetter : IContractProfileGetter
    {
        private readonly IInternalDataGetter _dataGetter;

        public ContractProfileGetter(IInternalDataGetter dataGetter)
        {
            _dataGetter = dataGetter;
        }

        public async Task<IPageItems<ContractProfile>> GetContractProfiles(FilterContract filterContract)
        {
            if (filterContract is null)
            {
                throw new ArgumentNullException(nameof(filterContract));
            }

            IQueryable<ContractProfile> contractProfilesQuery = _dataGetter
                .GetQueriable<ContractProfile>()
                .AsNoTracking()
                .Search(filterContract.SearchFilters);

            IList<ContractProfile> contracts = await contractProfilesQuery
                .Sort(filterContract.SortFilters)
                .Paginations(filterContract.PaginationFilter)
                .ToListAsync();

            return new PageItems<ContractProfile>(contracts, await contractProfilesQuery.CountAsync());
        }

        public async Task<ContractProfile> GetContractProfile(Guid contractProfilesId)
        {
            ContractProfile contract = await _dataGetter
                .GetQueriable<ContractProfile>()
                .AsNoTracking()
                .Include(c => c.RoleNotificationSettings)
                .Include(c => c.UserNotificationSettings)
                .Include(c => c.ContractSettingLines)
                    .ThenInclude(c => c.ContractPropperties)
                .SingleAsync(s => s.Id == contractProfilesId);

            return contract;
        }

        public Task<IEnumerable<ContractProfile>> GetContractProfilesWithWebhooks(string eventType)
        {
            return _dataGetter
                .GetQueriable<ContractProfile>()
                .AsNoTracking()
                .Where(c => c.NotifyEventType == eventType)
                .Where(c => c.UserNotificationSettings.Any(h => h.Enable) || c.RoleNotificationSettings.Any(h => h.Enable) || c.WebHooks.Any(h => h.Enable))
                .Include(c => c.ContractSettingLines.Where(c => c.Enable))
                    .ThenInclude(c => c.ContractPropperties)
                .Include(c => c.WebHooks.Where(h => h.Enable))
                .ToArrayAsync()
                .ContinueWith(a => a.Result.AsEnumerable());
        }

        public async Task<IEnumerable<UserSendView>> GetUsersFromContractProfiles(IEnumerable<Guid> contractProfileIds, string objectId)
        {
            List<UserSendView> userSendViews = await _dataGetter
                .GetQueriable<ContractProfile>()
                .AsNoTracking()
                .Where(p => contractProfileIds.Contains(p.Id))
                .SelectMany(p => p.UserNotificationSettings
                    .Where(u => u.Enable)
                    .Where(u => !(u.User.UserLock || u.User.UserExpiration < DateTime.UtcNow || u.User.PasswordExpiration < DateTime.UtcNow))
                    .Where(u =>
                        string.IsNullOrWhiteSpace(objectId) ||
                        u.User.UserClaims.Any(c =>
                            c.ClaimType == ResourceTag.AccessObjectIds &&
                            c.ClaimValue == objectId
                        )
                    )
                    .Select(u => new UserSendView
                    {
                        ContractProfileId = p.Id,
                        Email = u.User.Email,
                        TargetNotifies = u.TargetNotifies,
                        UserName = u.User.UserName,
                    })
                )
                .ToListAsync();

            string[] selectedUserNames = userSendViews.Select(u => u.UserName).ToArray();

            IEnumerable<IGrouping<Guid, UserGroupView>> roleGroupViews = await _dataGetter
                .GetQueriable<RoleNotificationSetting>()
                .Where(s => contractProfileIds.Contains(s.ContractProfileId))
                .Where(s => s.Enable)
                .Where(s => s.Role.RoleExpiration > DateTime.UtcNow)
                .Select(s => new UserGroupView(
                    s.ContractProfileId,
                    s.TargetNotifies,
                    s.Role.UserRoles
                        .AsQueryable()
                        .Where(u => u.User.SubdivisionId == s.SubdivisionId)
                        .Where(u => !selectedUserNames.Contains(u.User.UserName))
                        .Where(u => !(u.User.UserLock || u.User.UserExpiration < DateTime.UtcNow || u.User.PasswordExpiration < DateTime.UtcNow))
                        .Where(u =>
                            string.IsNullOrWhiteSpace(objectId) ||
                            u.User.UserClaims.Any(c =>
                                c.ClaimType == ResourceTag.AccessObjectIds &&
                                c.ClaimValue == objectId
                            )
                        )
                        .Select(u => new UserSendView
                        {
                            Email = u.User.Email,
                            UserName = u.User.UserName,
                        })
                        .ToList()
                    )
                )
                .ToArrayAsync()
                .ContinueWith(t => t.Result.GroupBy(t => t.ContractProfileId));

            foreach (IGrouping<Guid, UserGroupView> userGroupViewByContract in roleGroupViews)
            {
                IEnumerable<string> uniqUserNames = userGroupViewByContract
                    .SelectMany(u => u.UserSend.Select(u => u.UserName))
                    .Distinct();

                IEnumerable<UserSendView> sendViews = uniqUserNames.Select(n => new UserSendView
                {
                    ContractProfileId = userGroupViewByContract.Key,
                    UserName = n,
                    Email = userGroupViewByContract
                        .First(c => c.UserSend.Any(u => u.UserName == n))
                        .UserSend
                        .First(u => u.UserName == n).Email,
                    TargetNotifies = userGroupViewByContract
                        .Where(c => c.UserSend.Any(s => s.UserName == n))
                        .SelectMany(u => u.TargetNotifies)
                        .Distinct(),
                });

                userSendViews.AddRange(sendViews);
            }

            return userSendViews;
        }

        public async Task<IEnumerable<ContractProfile>> GetUserContractProfiles(Guid userId)
        {
            IEnumerable<ContractProfile> userProfiles = await _dataGetter
                .GetQueriable<ContractProfile>()
                .AsNoTracking()
                .Include(p => p.ContractSettingLines)
                    .ThenInclude(l => l.ContractPropperties)
                .Where(p => p.UserNotificationSettings.Any(u => u.UserId == userId))
                .ToListAsync()
                .ContinueWith(t => t.Result.AsEnumerable());

            string[] selectedNotifyEventTypes = userProfiles.Select(u => u.NotifyEventType).ToArray();

            IEnumerable<ContractProfile> roleProfiles = await _dataGetter
                .GetQueriable<ContractProfile>()
                .AsNoTracking()
                .Include(p => p.ContractSettingLines)
                    .ThenInclude(l => l.ContractPropperties)
                .Where(p =>
                    !selectedNotifyEventTypes.Contains(p.NotifyEventType) &&
                    p.RoleNotificationSettings.Any(s =>
                        s.Subdivision.Users.Any(u => u.Id == userId) &&
                        s.Role.UserRoles.Any(u => u.UserId == userId) &&
                        s.Role.RoleExpiration > DateTime.UtcNow
                    )
                )
                .ToListAsync()
                .ContinueWith(t => t.Result.AsEnumerable());

            return userProfiles.Concat(roleProfiles);
        }
    }
}
