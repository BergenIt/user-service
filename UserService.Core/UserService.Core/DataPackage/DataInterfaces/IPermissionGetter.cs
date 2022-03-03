using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    /// <summary>
    /// Получение групп ресурсов
    /// </summary>
    public interface IPermissionGetter
    {
        /// <summary>
        /// Получить все системные ресурсы
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ResourceTag>> GetSystemResources();

        /// <summary>
        /// Получить страницу с группами ресурсов
        /// </summary>
        /// <param name="filterContract"></param>
        /// <returns></returns>
        Task<IPageItems<Permission>> GetPermissionPage(FilterContract filterContract);

        /// <summary>
        ///  Получить группу ресурсов (с включенными сущностями)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Permission> GetPermission(Guid id);

        /// <summary>
        /// Получить группы ресурсов по роле
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IEnumerable<Permission>> GetRolePermissions(Guid roleId);

        /// <summary>
        /// Получить ресурсы к которым имеет доступ роль
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IEnumerable<ResourceTag>> GetRoleResources(Guid roleId);

        /// <summary>
        /// Получить права доступа роли (ресурсы + уровень)
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IEnumerable<Claim>> GetRoleAccess(Guid roleId);
    }
}

