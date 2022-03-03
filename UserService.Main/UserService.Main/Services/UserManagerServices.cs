using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using DatabaseExtension;

using Grpc.Core;

using UserService.Core.DataInterfaces;
using UserService.Core.UserManager;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    /// Сервис работы с пользователями
    /// </summary>
    public class UserManagerServices : UserManagerService.UserManagerServiceBase
    {
        private readonly IMapper _mapper;

        private readonly IUserGetter _userGetter;
        private readonly IUserManager _userManager;

        /// <summary>
        /// Сервис работы с пользователями
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="userGetter"></param>
        /// <param name="userManager"></param>
        public UserManagerServices(IMapper mapper, IUserGetter userGetter, IUserManager userManager)
        {
            _mapper = mapper;
            _userGetter = userGetter;
            _userManager = userManager;
        }

        /// <summary>
        /// Получить пользователя по имени
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<User> GetUserByUserName(UserGetRequest request, ServerCallContext context)
        {
            Core.Entity.User user = await _userGetter.GetUser(request.UserName);

            IEnumerable<Core.Entity.Role> roles = await _userGetter.GetUserRoles(user.Id, false);

            user.UserRoles = new List<Core.Entity.UserRole>(roles.Select(r => r.UserRoles.Single()));

            return _mapper.Map<User>(user);
        }

        /// <summary>
        /// Получить пользователей
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserPage> GetUsers(UsersGetRequest request, ServerCallContext context)
        {
            FilterContract filter = request.Filter.FromProtoFilter<User, Core.Entity.User>();

            IPageItems<Core.Entity.User> users = await _userGetter.GetUsers(filter);

            return new()
            {
                UserList = { _mapper.Map<IEnumerable<User>>(users.Items) },
                CountItems = (int)users.CountItems
            };
        }

        /// <summary>
        /// Создать пользователей
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Users> CreateUsers(UsersCreateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.UserManager.UserCreateCommand> usersCommands = _mapper.Map<IEnumerable<Core.UserManager.UserCreateCommand>>(request.CreateUsersList);

            IEnumerable<Core.Entity.User> users = await _userManager.CreateUsers(usersCommands);

            return new()
            {
                UserList = { _mapper.Map<IEnumerable<User>>(users) },
            };
        }

        /// <summary>
        /// Обновить пользователей
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Users> UpdateUsers(UsersUpdateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.UserManager.UserUpdateCommand> usersCommands = _mapper.Map<IEnumerable<Core.UserManager.UserUpdateCommand>>(request.UpdateUsersList);

            IEnumerable<Core.Entity.User> users = await _userManager.UpdateUsers(usersCommands);

            return new()
            {
                UserList = { _mapper.Map<IEnumerable<User>>(users) },
            };
        }

        /// <summary>
        /// Удалить пользователей
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Users> RemoveUsers(UsersRemoveCommand request, ServerCallContext context)
        {
            IEnumerable<Guid> removeIds = request.RemoveUsersId.Select(i => Guid.Parse(i));

            IEnumerable<Core.Entity.User> users = await _userManager.RemoveUsers(removeIds);

            return new()
            {
                UserList = { _mapper.Map<IEnumerable<User>>(users) },
            };
        }
    }
}
