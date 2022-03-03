using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension;

using Microsoft.EntityFrameworkCore;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

using static UserService.Core.DataInterfaces.IUserGetter;

namespace UserService.Data.EntityWorkers
{
    public class UserGetter : IUserGetter
    {
        private readonly IInternalDataGetter _dataGetter;

        public UserGetter(IInternalDataGetter dataGetter)
        {
            _dataGetter = dataGetter;
        }

        public Task<User> GetUser(Guid id, bool tracking = true)
        {
            IQueryable<User> users = _dataGetter.GetQueriable<User>();

            if (!tracking)
            {
                users = users.AsNoTracking();
            }

            return users.SingleAsync(u => u.Id == id);
        }

        public Task<User> GetUser(string userName, bool tracking = true)
        {
            IQueryable<User> users = _dataGetter.GetQueriable<User>();

            if (!tracking)
            {
                users = users.AsNoTracking();
            }

            return users.SingleAsync(u => u.UserName == userName);
        }

        public Task<Guid> GetUserId(string userName)
        {
            return _dataGetter.GetQueriable<User>().Where(u => u.UserName == userName).Select(u => u.Id).SingleAsync();
        }

        public Task<IEnumerable<Role>> GetUserRoles(User user, bool tracking = true)
        {
            IQueryable<Role> roles = _dataGetter
                .GetQueriable<Role>()
                .Where(r => r.UserRoles.Any(r => r.UserId == user.Id))
                .Include(r => r.UserRoles.Where(r => r.UserId == user.Id));

            if (!tracking)
            {
                roles = roles.AsNoTracking();
            }

            return roles.ToListAsync().ContinueWith(x => x.Result.AsEnumerable());
        }

        public Task<IEnumerable<Role>> GetUserRoles(Guid id, bool tracking = true)
        {
            IQueryable<Role> roles = _dataGetter
                .GetQueriable<Role>()
                .Where(r => r.UserRoles.Any(r => r.UserId == id))
                .Include(r => r.UserRoles.Where(r => r.UserId == id));

            if (!tracking)
            {
                roles = roles.AsNoTracking();
            }

            return roles.ToListAsync().ContinueWith(x => x.Result.AsEnumerable());
        }

        public Task<IEnumerable<Role>> GetUserRoles(string userName, bool tracking = true)
        {
            IQueryable<Role> roles = _dataGetter
                .GetQueriable<Role>()
                .Where(r => r.UserRoles.Any(r => r.User.UserName == userName))
                .Include(r => r.UserRoles.Where(r => r.User.UserName == userName));

            if (!tracking)
            {
                roles = roles.AsNoTracking();
            }

            return roles.ToListAsync().ContinueWith(x => x.Result.AsEnumerable());
        }

        public async Task<IPageItems<User>> GetUsers(FilterContract filter)
        {
            IQueryable<User> userQuery = _dataGetter
                .GetQueriable<User>()
                .Search(filter.SearchFilters);

            int count = await userQuery.CountAsync();

            IEnumerable<User> users = await userQuery
                .Sort(filter.SortFilters)
                .Paginations(filter.PaginationFilter)
                .Include(u => u.UserRoles)
                    .ThenInclude(r => r.Role)
                .ToListAsync();

            return new PageItems<User>(users, count);
        }

        public Task<string> GetUserSecurityKey(string userName)
        {
            return _dataGetter
                .GetQueriable<User>()
                .Where(u => u.UserName == userName)
                .Select(u => u.SecurityStamp)
                .SingleAsync();
        }

        public async Task<UserExist> UserExistAsync(string userName)
        {
            IQueryable<User> userQuery = _dataGetter
                .GetQueriable<User>()
                .Where(u => u.UserName == userName);

            if (!await userQuery.AnyAsync())
            {
                return UserExist.NotFound;
            }

            string passwordHash = await userQuery
                .Select(u => u.PasswordHash)
                .SingleOrDefaultAsync();

            return string.IsNullOrWhiteSpace(passwordHash) ? UserExist.Ldap : UserExist.Exist;
        }
    }
}
