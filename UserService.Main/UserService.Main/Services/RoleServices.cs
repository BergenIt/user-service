using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using DatabaseExtension;

using Grpc.Core;

using UserService.Core.DataInterfaces;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    /// Сервис работы с ролями
    /// </summary>
    public class RoleServices : RoleService.RoleServiceBase
    {
        private readonly IMapper _mapper;
        private readonly IRoleManager _roleManager;
        private readonly IRoleClaimManager _roleClaimManager;

        /// <summary>
        /// Сервис работы с ролями
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="roleManager"></param>
        /// <param name="roleClaimManager"></param>
        public RoleServices(IMapper mapper, IRoleManager roleManager, IRoleClaimManager roleClaimManager)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _roleClaimManager = roleClaimManager;
        }

        /// <summary>
        /// Получить роль по id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Role> GetRoleById(RoleGetRequest request, ServerCallContext context)
        {
            Guid roleId = Guid.Parse(request.Id);

            Core.Entity.Role role = await _roleManager.GetRole(roleId);

            return _mapper.Map<Role>(role);
        }

        /// <summary>
        /// Получить роли
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RolePage> GetRoles(RolesGetRequest request, ServerCallContext context)
        {
            FilterContract filter = request.Filter.FromProtoFilter<Role, Core.Entity.Role>();

            IPageItems<Core.Entity.Role> roles = await _roleManager.GetRoles(filter);

            return new()
            {
                RoleList = { _mapper.Map<IEnumerable<Role>>(roles.Items) },
                CountItems = (int)roles.CountItems
            };
        }

        /// <summary>
        /// Создать роли
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Roles> CreateRoles(RolesCreateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.Role> roles = _mapper.Map<IEnumerable<Core.Entity.Role>>(request.CreateRolesList);

            IEnumerable<Core.Entity.Role> addedRoles = await _roleManager.AddRoles(roles);

            return new()
            {
                RoleList = { _mapper.Map<IEnumerable<Role>>(addedRoles) }
            };
        }

        /// <summary>
        /// Обновить роли
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Roles> UpdateRoles(RolesUpdateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.Role> roles = _mapper.Map<IEnumerable<Core.Entity.Role>>(request.UpdateRolesList);

            IEnumerable<Core.Entity.Role> updatedRoles = await _roleManager.UpdateRoles(roles);

            return new()
            {
                RoleList = { _mapper.Map<IEnumerable<Role>>(updatedRoles) }
            };
        }

        /// <summary>
        /// Удалить роли
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Roles> RemoveRoles(RolesRemoveCommand request, ServerCallContext context)
        {
            IEnumerable<Guid> roleIds = request.RemoveRolesId.Select(r => Guid.Parse(r));

            IEnumerable<Core.Entity.Role> removedRoles = await _roleManager.RemoveRoles(roleIds);

            return new()
            {
                RoleList = { _mapper.Map<IEnumerable<Role>>(removedRoles) }
            };
        }

        /// <summary>
        /// Получить права доступа ролей к ресурсам
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RoleLevelAccessList> GetAssertLevelRole(GetRoleLevelAccessRequest request, ServerCallContext context)
        {
            Guid roleId = Guid.Parse(request.RoleId);

            IEnumerable<Core.Entity.RoleClaim> roleClaims = await _roleClaimManager.GetRoleClaims(roleId);

            return new()
            {
                RoleLevelAccessList_ = { _mapper.Map<IEnumerable<RoleLevelAccess>>(roleClaims) },
            };
        }

        /// <summary>
        /// Изменить права доступа ролей к ресурсам
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RoleLevelAccessList> ChangeAssertLevelRole(ChangeRoleLevelAccessCommand request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.RoleClaim> inputRoleClaims = _mapper.Map<IEnumerable<Core.Entity.RoleClaim>>(request.RoleLevelAccessList);

            _ = await _roleClaimManager.ChangeAssertLevelRoleClaims(inputRoleClaims);

            return new()
            {
                RoleLevelAccessList_ = { request.RoleLevelAccessList },
            };
        }
    }
}
