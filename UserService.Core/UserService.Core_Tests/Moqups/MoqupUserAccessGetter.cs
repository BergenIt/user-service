using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupUserAccessGetter : IUserAccessGetter
    {
        private readonly List<Claim> _claims = new()
        {
            new Claim(ResourceTag.AccessObjectIds, "test"),
            new Claim(ResourceTag.AccessObjectIds, "test_second"),
        };

        public Task<ICollection<Claim>> GetUserClaims(Guid userId)
        {
            Assert.IsTrue(userId != Guid.Empty);

            return Task.FromResult(_claims as ICollection<Claim>);
        }
    }
}
