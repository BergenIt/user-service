using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UserService.Core.Entity;
using UserService.Core.UserManager;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsIUserManager : IUserManager
    {
        private readonly ObjectFabric _objectFabric = new();

        public List<string> AddedUserNames { get; } = new();
        public List<Guid> UpdatedEntitys { get; } = new();
        public List<Guid> RemovedEntitys { get; } = new();

        public bool Saved { get; }

        public Task AddUserScreenTime(string userName, DateTime startSession)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> CreateUsers(IEnumerable<UserCreateCommand> userCreateCommands)
        {
            AddedUserNames.AddRange(userCreateCommands.Select(e => e.UserName));

            return Task.FromResult(userCreateCommands.Select(u => new User { UserName = u.UserName, Id = Guid.NewGuid() }));
        }

        public Task<IEnumerable<User>> RemoveUsers(IEnumerable<Guid> ids)
        {
            RemovedEntitys.AddRange(ids);

            return Task.FromResult(ids.Select(i => _objectFabric.CreateInstance<User>(i, 2)));
        }

        public Task<IEnumerable<User>> UpdateUsers(IEnumerable<UserUpdateCommand> userUpdateCommands)
        {
            UpdatedEntitys.AddRange(userUpdateCommands.Select(e => e.Id));

            return Task.FromResult(userUpdateCommands.Select(u => new User { Id = u.Id }));
        }
    }
}
