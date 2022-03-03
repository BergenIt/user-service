using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    /// <summary>
    /// Получение пользователей
    /// </summary>
    public interface IUserGetter
    {
        /// <summary>
        /// Проверка существования пользователя
        /// </summary>
        public enum UserExist
        {
            /// <summary>
            /// Не существует
            /// </summary>
            NotFound,

            /// <summary>
            /// Существует и подключен из АД
            /// </summary>
            Ldap,

            /// <summary>
            /// Существует и является локальным
            /// </summary>
            Exist,
        }

        /// <summary>
        /// Проверка существования юзера
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<UserExist> UserExistAsync(string userName);

        /// <summary>
        /// Получить страницу с пользователями
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IPageItems<User>> GetUsers(FilterContract filter);

        /// <summary>
        /// Получить роли пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        Task<IEnumerable<Role>> GetUserRoles(User user, bool tracking = true);

        /// <summary>
        /// Получить роли пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        Task<IEnumerable<Role>> GetUserRoles(Guid id, bool tracking = true);

        /// <summary>
        /// Получить роли пользователя
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        Task<IEnumerable<Role>> GetUserRoles(string userName, bool tracking = true);

        /// <summary>
        /// Получить юзера
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        Task<User> GetUser(Guid id, bool tracking = true);

        /// <summary>
        /// Получить юзера
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        Task<User> GetUser(string userName, bool tracking = true);


        /// <summary>
        /// Получить id юзера по его имени
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<Guid> GetUserId(string userName);

        /// <summary>
        /// Получить секретШтамп юзера
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<string> GetUserSecurityKey(string userName);
    }
}
