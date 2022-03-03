using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsUserGetter : IUserGetter
    {
        public const string DefaultValue = "DefaultValue";

        private readonly ObjectFabric _objectFabric = new();

        public Task<IEnumerable<Claim>> GetAccessObjectIds(string userName)
        {
            return Task.FromResult(new Claim[] { new Claim(ResourceTag.AccessObjectIds, Guid.NewGuid().ToString()) }.AsEnumerable());
        }

        public Task<User> GetUser(Guid id, bool tracking = true)
        {
            return Task.FromResult(GenerateUser(id));
        }

        public Task<User> GetUser(string userName, bool tracking = true)
        {
            return Task.FromResult(GenerateUser(userName));
        }

        public Task<Guid> GetUserId(string userName)
        {
            return Task.FromResult(GenerateUser(userName).Id);
        }

        public Task<IEnumerable<Permission>> GetUserPermission(string userName, bool tracking = true)
        {
            return Task.FromResult(new List<Permission> { _objectFabric.CreateInstance<Permission>(2), _objectFabric.CreateInstance<Permission>(2) }.AsEnumerable());
        }

        public Task<IEnumerable<Role>> GetUserRoles(User user, bool tracking = true)
        {
            return Task.FromResult(new List<Role> { _objectFabric.CreateInstance<Role>(2), _objectFabric.CreateInstance<Role>(2) }.AsEnumerable());
        }

        public Task<IEnumerable<Role>> GetUserRoles(Guid id, bool tracking = true)
        {
            return Task.FromResult(new List<Role> { _objectFabric.CreateInstance<Role>(2), _objectFabric.CreateInstance<Role>(2) }.AsEnumerable());
        }

        public Task<IEnumerable<Role>> GetUserRoles(string userName, bool tracking = true)
        {
            return Task.FromResult(new List<Role> { _objectFabric.CreateInstance<Role>(2), _objectFabric.CreateInstance<Role>(2) }.AsEnumerable());
        }

        public Task<IPageItems<User>> GetUsers(FilterContract filterContract)
        {
            return Task.FromResult(new PageItems<User>(new List<User> { _objectFabric.CreateInstance<User>(2), _objectFabric.CreateInstance<User>(2) }, 2) as IPageItems<User>);
        }

        public Task<string> GetUserSecurityKey(string userName)
        {
            return Task.FromResult(DefaultValue);
        }

        public Task<IUserGetter.UserExist> UserExistAsync(string userName)
        {
            return Task.FromResult(IUserGetter.UserExist.Exist);
        }

        private User GenerateUser(string userName) => new()
        {
            Id = Guid.NewGuid(),
            Description = Guid.NewGuid().ToString(),
            UserExpiration = DateTime.MaxValue,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString(),
            FullName = Guid.NewGuid().ToString(),
            LastLogin = DateTime.UtcNow,
            NormalizedEmail = Guid.NewGuid().ToString(),
            NormalizedUserName = userName,
            UserName = userName,
            PasswordHash = Guid.NewGuid().ToString(),
            SecurityStamp = DefaultValue,
            PositionId = Guid.NewGuid(),
            SubdivisionId = Guid.NewGuid(),
            UserLock = false,
            PasswordExpiration = DateTime.MaxValue,
        };

        private User GenerateUser(Guid id) => new()
        {
            Id = id,
            Description = Guid.NewGuid().ToString(),
            UserExpiration = DateTime.MaxValue,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString(),
            FullName = Guid.NewGuid().ToString(),
            LastLogin = DateTime.UtcNow,
            NormalizedEmail = Guid.NewGuid().ToString(),
            NormalizedUserName = Guid.NewGuid().ToString(),
            UserName = Guid.NewGuid().ToString(),
            PasswordHash = Guid.NewGuid().ToString(),
            SecurityStamp = DefaultValue,
            PositionId = Guid.NewGuid(),
            SubdivisionId = Guid.NewGuid(),
            UserLock = false,
            PasswordExpiration = DateTime.MaxValue,
        };
    }
}
