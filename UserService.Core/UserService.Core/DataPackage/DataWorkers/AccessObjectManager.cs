using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core.DataPackage.DataWorkers
{
    public class AccessObjectManager : IAccessObjectManager
    {
        private readonly IDataGetter _dataGetter;
        private readonly IDataWorker _dataWorker;

        public AccessObjectManager(IDataGetter dataGetter, IDataWorker dataWorker)
        {
            _dataGetter = dataGetter;
            _dataWorker = dataWorker;
        }

        public Task<IEnumerable<Guid>> GetAccessObjectUsers(string accessObjectId)
        {
            return _dataGetter
                .GetEntitiesAsync<UserClaim>(r => r.ClaimValue == accessObjectId)
                .ContinueWith(t => t.Result.Select(c => c.UserId).Distinct());
        }

        public Task<IEnumerable<string>> GetUserAccessObjects(Guid userId)
        {
            return _dataGetter
                .GetEntitiesAsync<UserClaim>(r => r.UserId == userId)
                .ContinueWith(t => t.Result.Select(c => c.ClaimValue).Distinct());
        }

        public async Task<IEnumerable<string>> AddAccessObjectsToUser(Guid userId, IEnumerable<string> accessObjectIds)
        {
            foreach (string accessObjectId in accessObjectIds)
            {
                await AddClaim(userId, accessObjectId);
            }

            await _dataWorker.SaveChangesAsync();

            return accessObjectIds;
        }

        public async Task<IEnumerable<Guid>> AddUsersToAccessObject(string accessObjectId, IEnumerable<Guid> userIds)
        {
            foreach (Guid userId in userIds)
            {
                await AddClaim(userId, accessObjectId);
            }

            await _dataWorker.SaveChangesAsync();

            return userIds;
        }

        public async Task<IEnumerable<string>> RemoveAccessObjectsFromUser(Guid userId, IEnumerable<string> accessObjectIds)
        {
            foreach (string accessObjectId in accessObjectIds)
            {
                await RemoveClaim(userId, accessObjectId);
            }

            await _dataWorker.SaveChangesAsync();

            return accessObjectIds;
        }

        public async Task<IEnumerable<Guid>> RemoveUsersFromAccessObject(string accessObjectId, IEnumerable<Guid> userIds)
        {
            foreach (Guid userId in userIds)
            {
                await RemoveClaim(userId, accessObjectId);
            }

            await _dataWorker.SaveChangesAsync();

            return userIds;
        }

        private Task AddClaim(Guid userId, string accessObjectId)
        {
            UserClaim userClaim = new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ClaimType = ResourceTag.AccessObjectIds,
                ClaimValue = accessObjectId,
            };

            return _dataWorker.AddAsync(userClaim);
        }

        private async Task RemoveClaim(Guid userId, string accessObjectId)
        {
            UserClaim userClaim = await _dataGetter.GetSingleEntityAsync<UserClaim>(c => c.UserId == userId && c.ClaimValue == accessObjectId && c.ClaimType == ResourceTag.AccessObjectIds);

            _ = _dataWorker.Remove(userClaim);
        }
    }
}
