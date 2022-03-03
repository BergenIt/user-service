using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core.DataPackage.DataWorkers
{
    public class UserAccessGetter : IUserAccessGetter
    {
        private readonly IPermissionGetter _permissionGetter;
        private readonly IUserGetter _userGetter;
        private readonly IAccessObjectManager _accessObjectManager;

        public UserAccessGetter(IPermissionGetter permissionGetter, IUserGetter userGetter, IAccessObjectManager accessObjectManager)
        {
            _permissionGetter = permissionGetter;
            _userGetter = userGetter;
            _accessObjectManager = accessObjectManager;
        }

        public async Task<ICollection<Claim>> GetUserClaims(Guid userId)
        {
            List<Claim> result = new();

            IEnumerable<Guid> roleIds = await _userGetter
                .GetUserRoles(userId, false)
                .ContinueWith(r => r.Result.Where(r => r.RoleExpiration > DateTime.UtcNow).Select(r => r.Id));

            foreach (Guid roleId in roleIds)
            {
                IEnumerable<Claim> claims = await _permissionGetter.GetRoleAccess(roleId);

                foreach (Claim claim in claims)
                {
                    Claim existClaim = result.SingleOrDefault(c => c.Type == claim.Type);

                    if (existClaim is null)
                    {
                        result.Add(claim);
                    }
                    else if (Enum.Parse<PermissionAssert>(claim.Value) > Enum.Parse<PermissionAssert>(existClaim.Value))
                    {
                        _ = result.Remove(existClaim);
                        result.Add(claim);
                    }
                }
            }

            IEnumerable<string> userAccessObjectIds = await _accessObjectManager.GetUserAccessObjects(userId);

            result.AddRange(userAccessObjectIds.Select(id => new Claim(ResourceTag.AccessObjectIds, id)));

            return result;
        }
    }
}
