using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.Entity;

namespace UserService.Core.UserManager
{
    /// <summary>
    /// Crud пользователей
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Удаляет юзеров
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<User>> RemoveUsers(IEnumerable<Guid> ids);

        /// <summary>
        /// Обновляет юзеров
        /// </summary>
        /// <param name="userUpdateCommands"></param>
        /// <returns></returns>
        Task<IEnumerable<User>> UpdateUsers(IEnumerable<UserUpdateCommand> userUpdateCommands);

        /// <summary>
        /// Создает юзеров
        /// </summary>
        /// <param name="userCreateCommands"></param>
        /// <returns></returns>
        Task<IEnumerable<User>> CreateUsers(IEnumerable<UserCreateCommand> userCreateCommands);
    }
}
