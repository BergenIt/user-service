using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core.DataPackage.DataWorkers
{
    public class RoleClaimManager : IRoleClaimManager
    {
        private readonly IDataGetter _dataGetter;
        private readonly IDataWorker _dataWorker;

        private readonly IRoleGetter _roleGetter;

        public RoleClaimManager(IDataGetter dataGetter, IDataWorker dataWorker, IRoleGetter roleGetter)
        {
            _dataGetter = dataGetter;
            _dataWorker = dataWorker;
            _roleGetter = roleGetter;
        }

        public async Task<IEnumerable<RoleClaim>> ChangeAssertLevelRoleClaims(IEnumerable<RoleClaim> inputRoleClaims)
        {
            List<RoleClaim> roleClaims = new();

            foreach (RoleClaim inputRoleClaim in inputRoleClaims)
            {
                RoleClaim roleClaim = await _dataGetter.GetSingleEntityAsync<RoleClaim>(c => c.RoleId == inputRoleClaim.RoleId && c.ClaimType == inputRoleClaim.ClaimType);

                roleClaim.PermissionAssert = inputRoleClaim.PermissionAssert;

                roleClaims.Add(roleClaim);
            }

            _ = _dataWorker.UpdateRange(roleClaims);

            await _dataWorker.SaveChangesAsync();

            return roleClaims;
        }

        public Task<IEnumerable<RoleClaim>> GetRoleClaims(Guid roleId)
        {
            return _roleGetter.GetRoleClaims(roleId);
        }
    }
}
