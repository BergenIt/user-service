using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    /// <summary>
    /// Управление ролями
    /// </summary>
    public interface IRoleManager
    {
        /// <summary>
        /// Получить страницу ролей
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IPageItems<Role>> GetRoles(FilterContract filter);

        /// <summary>
        /// Получить роль
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Role> GetRole(Guid id);

        /// <summary>
        /// Создать роли
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        Task<IEnumerable<Role>> AddRoles(IEnumerable<Role> roles);

        /// <summary>
        /// Удалить роли
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        Task<IEnumerable<Role>> RemoveRoles(IEnumerable<Guid> roles);

        /// <summary>
        /// Создать роли
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        Task<IEnumerable<Role>> UpdateRoles(IEnumerable<Role> roles);
    }
}
