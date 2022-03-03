using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core_Tests.Moqups
{
    internal class MoqupsRoleGetter : IRoleGetter
    {
        public Task<IEnumerable<RoleClaim>> GetRoleClaims(Guid roleId)
        {
            List<RoleClaim> roleClaims = new() { GenerateRoleClaim() };

            return Task.FromResult(roleClaims.AsEnumerable());
        }

        RoleClaim GenerateRoleClaim() => new()
        {
            RoleId = Guid.NewGuid(),
            ClaimType = Guid.NewGuid().ToString(),
            ClaimValue = Guid.NewGuid().ToString(),
            Id = Guid.NewGuid(),
            PermissionAssert = new(),
            ResourceTag = new(),
            Role = new(),
        };
    }
}
