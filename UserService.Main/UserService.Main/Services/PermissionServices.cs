using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using DatabaseExtension;

using Grpc.Core;

using UserService.Core.DataInterfaces;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    /// Сервис работы с группами ресурсов
    /// </summary>
    public class PermissionServices : PermissionService.PermissionServiceBase
    {
        private readonly IMapper _mapper;

        private readonly IPermissionManager _permissionManager;
        private readonly IPermissionGetter _permissionGetter;

        /// <summary>
        /// Сервис работы с группами ресурсов
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="permissionManager"></param>
        /// <param name="permissionGetter"></param>
        public PermissionServices(IMapper mapper, IPermissionManager permissionManager, IPermissionGetter permissionGetter)
        {
            _mapper = mapper;
            _permissionManager = permissionManager;
            _permissionGetter = permissionGetter;
        }

        /// <summary>
        /// Получить группу ресурсов
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Permission> GetPermission(GetPermissionRequest request, ServerCallContext context)
        {
            Guid id = Guid.Parse(request.Id);

            Core.Entity.Permission permission = await _permissionGetter.GetPermission(id);

            return _mapper.Map<Permission>(permission);
        }

        /// <summary>
        /// Получить страницу с группами ресурсов
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<PermissionPage> GetPermissionPage(GetPermissionPageRequest request, ServerCallContext context)
        {
            FilterContract filterContract = request.Filter.FromProtoFilter();

            IPageItems<Core.Entity.Permission> pageItems = await _permissionGetter.GetPermissionPage(filterContract);

            return new()
            {
                CountItems = (int)pageItems.CountItems,
                PermissionList = { _mapper.Map<IEnumerable<Permission>>(pageItems.Items) }
            };
        }

        /// <summary>
        /// Получить системные ресурсы
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Resources> GetSystemResources(GetSystemResourcesRequest request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.ResourceTag> resourceTags = await _permissionGetter.GetSystemResources();

            return new()
            {
                Resources_ = { _mapper.Map<IEnumerable<Resource>>(resourceTags) }
            };
        }

        /// <summary>
        /// Получить группы ресурсов роли
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Permissions> GetRolePermissions(GetRolePermissionsRequest request, ServerCallContext context)
        {
            Guid roleId = Guid.Parse(request.RoleId);

            IEnumerable<Core.Entity.Permission> permissions = await _permissionGetter.GetRolePermissions(roleId);

            return new()
            {
                PermissionList = { _mapper.Map<IEnumerable<Permission>>(permissions) }
            };
        }

        /// <summary>
        /// Изменить список ролей группы ресурсов
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Permission> ChangePermissionRoles(ChangePermissionRolesCommand request, ServerCallContext context)
        {
            Guid id = Guid.Parse(request.PermissionId);

            IEnumerable<Guid> roleIds = _mapper.Map<IEnumerable<Guid>>(request.RoleIds);

            Core.Entity.Permission permission = await _permissionManager.ChangePermissionRoles(id, roleIds);

            return _mapper.Map<Permission>(permission);
        }

        /// <summary>
        /// Создать группу ресурсов
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Permission> CreatePermission(PermissionCreateCommand request, ServerCallContext context)
        {
            Core.Entity.Permission permissionInput = _mapper.Map<Core.Entity.Permission>(request);

            Core.Entity.Permission permission = await _permissionManager.CreatePermission(permissionInput);

            return _mapper.Map<Permission>(permission);
        }

        /// <summary>
        /// Обновить группу ресурсов
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Permission> UpdatePermission(PermissionUpdateCommand request, ServerCallContext context)
        {
            Core.Entity.Permission permissionInput = _mapper.Map<Core.Entity.Permission>(request);

            Core.Entity.Permission permission = await _permissionManager.UpdatePermission(permissionInput);

            return _mapper.Map<Permission>(permission);
        }

        /// <summary>
        /// Удалить группы ресурсов
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Permissions> RemoveRolePermissions(PermissionsRemoveCommand request, ServerCallContext context)
        {
            IEnumerable<Guid> removeIds = _mapper.Map<IEnumerable<Guid>>(request.RemovePermissionsId);

            IEnumerable<Core.Entity.Permission> permissions = await _permissionManager.RemovePermissions(removeIds);

            return new()
            {
                PermissionList = { _mapper.Map<IEnumerable<Permission>>(permissions) }
            };
        }
    }
}
