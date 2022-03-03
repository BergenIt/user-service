using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    /// <summary>
    /// Управление группами ресурсов
    /// </summary>
    public interface IPermissionManager
    {
        /// <summary>
        /// Создать группу ресурсов
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task<Permission> CreatePermission(Permission permission);

        /// <summary>
        /// Обновление группы ресурсов
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task<Permission> UpdatePermission(Permission permission);

        /// <summary>
        /// Обновление связи группы ресурсов с ролями
        /// </summary>
        /// <param name="permissionId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        Task<Permission> ChangePermissionRoles(Guid permissionId, IEnumerable<Guid> roleIds);

        /// <summary>
        /// Удаление групп ресурсов
        /// </summary>
        /// <param name="permissionIds"></param>
        /// <returns></returns>
        Task<IEnumerable<Permission>> RemovePermissions(IEnumerable<Guid> permissionIds);
    }
}
