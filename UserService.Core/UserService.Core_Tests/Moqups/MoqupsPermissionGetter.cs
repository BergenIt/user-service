using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using DatabaseExtension;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsPermissionGetter : IPermissionGetter
    {
        private const string Default = "test";

        public readonly Permission DefaultPermission = new()
        {
            Id = Guid.NewGuid(),
            Comment = Default,
            Name = Default,
        };

        public readonly Claim DefaultClaim = new(Default, nameof(PermissionAssert.Remove));

        public readonly ResourceTag DefaultResourceTag = new()
        {
            Id = Guid.NewGuid(),
            Name = Default,
            Tag = Default,
        };

        public Task<Permission> GetPermission(Guid id)
        {
            Assert.IsTrue(id != default);

            throw new NotImplementedException();
        }

        public Task<IPageItems<Permission>> GetPermissionPage(FilterContract filterContract)
        {
            Assert.IsTrue(filterContract is not null);

            return Task.FromResult(new PageItems<Permission>(new Permission[] { DefaultPermission }, 2) as IPageItems<Permission>);
        }

        public Task<IEnumerable<Claim>> GetRoleAccess(Guid roleId)
        {
            Assert.IsTrue(roleId != default);

            return Task.FromResult(new Claim[] { DefaultClaim }.AsEnumerable());
        }

        public Task<IEnumerable<Permission>> GetRolePermissions(Guid roleId)
        {
            Assert.IsTrue(roleId != default);

            return Task.FromResult(new Permission[] { DefaultPermission }.AsEnumerable());
        }

        public Task<IEnumerable<ResourceTag>> GetRoleResources(Guid roleId)
        {
            Assert.IsTrue(roleId != default);

            return Task.FromResult(new ResourceTag[] { DefaultResourceTag }.AsEnumerable());
        }

        public Task<IEnumerable<ResourceTag>> GetSystemResources()
        {
            return Task.FromResult(new ResourceTag[] { DefaultResourceTag }.AsEnumerable());
        }
    }
}
