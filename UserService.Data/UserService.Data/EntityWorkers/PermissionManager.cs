using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using UserService.Core.DataInterfaces;
using UserService.Core.DataPackage;
using UserService.Core.Entity;
using UserService.Core.Models;

namespace UserService.Data.EntityWorkers
{
    public class PermissionManager : ManyToManyHandler<Permission>, IPermissionManager
    {
        private readonly IDataWorker _dataWorker;
        private readonly IInternalDataGetter _internalDataGetter;

        public PermissionManager(IDataWorker dataWorker, IInternalDataGetter dataGetter) : base(dataGetter)
        {
            _dataWorker = dataWorker;
            _internalDataGetter = dataGetter;
        }

        public async Task<Permission> ChangePermissionRoles(Guid permissionId, IEnumerable<Guid> roleIds)
        {
            Permission permission = await _internalDataGetter
                .GetQueriable<Permission>()
                .Include(p => p.Roles)
                .SingleAsync(p => p.Id == permissionId);

            ChangeRelationResult<Role> changeRoles = await ChangeRelations(permission, roleIds, p => p.Roles);

            _ = _dataWorker.Update(permission);
            await _dataWorker.SaveChangesAsync();

            IEnumerable<string> tags = await GetAllPermissionResorces(permission.Id);

            await ChangeRoleResourceHandle(tags, changeRoles.ChangeEntityIds);
            await _dataWorker.SaveChangesAsync();

            return permission;
        }

        public async Task<Permission> CreatePermission(Permission permission)
        {
            permission.Id = Guid.NewGuid();

            IEnumerable<Guid> resourceTagIds = permission.ResourceTags.Select(t => t.Id).ToArray();
            permission.ResourceTags.Clear();

            IEnumerable<Guid> lockedResourceTagIds = permission.LockedResourceTags.Select(t => t.Id).ToArray();
            permission.LockedResourceTags.Clear();

            IEnumerable<Guid> motherIds = permission.MotherPermissions.Select(t => t.Id).ToArray();
            permission.MotherPermissions.Clear();

            IEnumerable<Guid> roleIds = permission.Roles.Select(t => t.Id).ToArray();
            permission.Roles.Clear();

            ChangeRelationResult<ResourceTag> resources = await ChangeRelations(permission, resourceTagIds, p => p.ResourceTags);
            ChangeRelationResult<ResourceTag> lockedResources = await ChangeRelations(permission, lockedResourceTagIds, p => p.LockedResourceTags);
            ChangeRelationResult<Permission> motherPermissions = await ChangeRelations(permission, motherIds, p => p.MotherPermissions);
            ChangeRelationResult<Role> changeRoles = await ChangeRelations(permission, roleIds, p => p.Roles);

            _ = await _dataWorker.AddAsync(permission);
            await _dataWorker.SaveChangesAsync();

            await ChangeRolesClaims(permission, changeRoles.RemovedEntities, resources.RemovedEntities, lockedResources.RemovedEntities, motherPermissions.RemovedEntities);
            await _dataWorker.SaveChangesAsync();

            return permission;
        }

        public async Task<IEnumerable<Permission>> RemovePermissions(IEnumerable<Guid> permissionIds)
        {
            IEnumerable<Permission> permissions = await _dataWorker.RemoveRangeAsync<Permission>(permissionIds);

            List<string> resources = new();
            List<Guid> roleIds = new();

            foreach (Permission permission in permissions)
            {
                List<Guid> rolesPermission = await _internalDataGetter.GetQueriable<Permission>()
                    .Where(p => p.Id == permission.Id)
                    .SelectMany(p => p.Roles.Select(r => r.Id))
                    .ToListAsync();

                roleIds.AddRange(await GetPermissionRoles(permission));
                roleIds.AddRange(rolesPermission);

                IEnumerable<string> tagsPermission = await GetAllPermissionResorces(permission.Id);

                resources.AddRange(tagsPermission);
            }

            await _dataWorker.SaveChangesAsync();

            await ChangeRoleResourceHandle(resources, roleIds);
            await _dataWorker.SaveChangesAsync();

            return permissions;
        }

        public async Task<Permission> UpdatePermission(Permission permission)
        {
            Permission existPermission = await _internalDataGetter
                .GetQueriable<Permission>()
                .Include(p => p.Roles)
                .Include(p => p.MotherPermissions)
                .Include(p => p.ResourceTags)
                .Include(p => p.LockedResourceTags)
                .SingleAsync(p => p.Id == permission.Id);

            existPermission.Name = permission.Name;
            existPermission.Comment = permission.Comment;

            ChangeRelationResult<Role> changeRoles = await ChangeRelations(existPermission, permission, p => p.Roles);
            ChangeRelationResult<ResourceTag> resources = await ChangeRelations(existPermission, permission, p => p.ResourceTags);
            ChangeRelationResult<ResourceTag> lockedResources = await ChangeRelations(existPermission, permission, p => p.LockedResourceTags);
            ChangeRelationResult<Permission> motherPermissions = await ChangeRelations(existPermission, permission, p => p.MotherPermissions);

            _ = _dataWorker.Update(existPermission);
            await _dataWorker.SaveChangesAsync();

            await ChangeRolesClaims(existPermission, changeRoles.RemovedEntities, resources.RemovedEntities, lockedResources.RemovedEntities, motherPermissions.RemovedEntities);
            await _dataWorker.SaveChangesAsync();

            return existPermission;
        }

        /// <summary>
        /// Проверяет роли по каждому ресурсу и обновляет список их клаймов
        /// </summary>
        /// <param name="permissionsResources">Все ресурсы для проверки</param>
        /// <param name="permissionsRoles">Все роли для проверки</param>
        /// <returns></returns>
        private async Task ChangeRoleResourceHandle(IEnumerable<string> permissionsResources, IEnumerable<Guid> permissionsRoles)
        {
            if (!permissionsRoles.Any() || !permissionsResources.Any())
            {
                return;
            }

            permissionsRoles = permissionsRoles.Distinct();
            permissionsResources = permissionsResources.Distinct();

            foreach (string resource in permissionsResources)
            {
                IEnumerable<Guid> roleAccessIds = await GetRolesWithAccess(permissionsRoles, resource);

                if (roleAccessIds.Any())
                {
                    IEnumerable<Guid> roleIdsNew = await _internalDataGetter.GetQueriable<Role>()
                    .Where(c => !c.RoleClaims.Any(r => r.ClaimType == resource) && roleAccessIds.Contains(c.Id))
                    .Select(r => r.Id)
                    .ToArrayAsync();

                    IEnumerable<RoleClaim> newClaims = roleIdsNew.Select(r => new RoleClaim
                    {
                        Id = Guid.NewGuid(),
                        RoleId = r,
                        PermissionAssert = PermissionAssert.Read,
                        ClaimType = resource,
                    });

                    await _dataWorker.AddRangeAsync(newClaims);
                }

                IEnumerable<Guid> removeRoleIds = permissionsRoles
                    .Where(r => !roleAccessIds.Contains(r));

                if (removeRoleIds.Any())
                {
                    IEnumerable<RoleClaim> rmClaims = await _dataGetter
                        .GetEntitiesAsync<RoleClaim>(c => c.ClaimType == resource && removeRoleIds.Contains(c.RoleId));

                    _ = _dataWorker.RemoveRange(rmClaims);
                }
            }
        }

        private async Task ChangeRolesClaims(Permission permision, IEnumerable<Role> rmRoles, IEnumerable<ResourceTag> rmResource, IEnumerable<ResourceTag> rmLockResource, IEnumerable<Permission> rmMothers)
        {
            List<string> resources = rmResource.Select(r => r.Tag).Concat(rmLockResource.Select(r => r.Tag)).ToList();

            foreach (Permission rmMother in rmMothers)
            {
                IEnumerable<string> rmMotnerResoureces = await GetAllPermissionResorces(rmMother.Id);

                resources.AddRange(rmMotnerResoureces);
            }

            List<Guid> roleIds = rmRoles.Select(r => r.Id).ToList();

            IEnumerable<Guid> permissionRoleIds = await GetPermissionRoles(permision);
            IEnumerable<string> permissionResources = await GetAllPermissionResorces(permision.Id);

            resources.AddRange(permissionResources);
            roleIds.AddRange(permissionRoleIds);

            await ChangeRoleResourceHandle(resources, roleIds);

            await _dataWorker.SaveChangesAsync();
        }

        /// <summary>
        /// Селектит все ресурсы permission (в том числе материнские и в том числе заблоченные)
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<string>> GetAllPermissionResorces(Guid permissionId)
        {
            IEnumerable<Guid> motherPermissionIds = await _dataGetter.GetRecoursiveEntityIds<Permission>(permissionId, p => p.MotherPermissions);

            IEnumerable<string> tags = await _internalDataGetter.GetQueriable<ResourceTag>()
                .Where(t =>
                    t.Permissions.Any(p => p.Id == permissionId) ||
                    t.LockedPermissions.Any(p => p.Id == permissionId) ||
                    t.Permissions.Any(p => motherPermissionIds.Contains(p.Id)) ||
                    t.LockedPermissions.Any(p => motherPermissionIds.Contains(p.Id))
                )
                .Select(t => t.Tag)
                .ToListAsync();

            return tags;
        }

        /// <summary>
        /// Получает роли зависимые от пермишина (в том числе и дочерние)
        /// </summary>
        /// <param name="permision"></param>
        /// <returns></returns>
        private async Task<IEnumerable<Guid>> GetPermissionRoles(Permission permision)
        {
            IEnumerable<Guid> childPermissionIds = await _dataGetter.GetRecoursiveEntityIds<Permission>(permision.Id, p => p.ChildPermissions);

            List<Guid> roleIds = await _internalDataGetter.GetQueriable<Role>()
                .Where(r => r.Permissions.Any(p => childPermissionIds.Contains(p.Id)))
                .Select(r => r.Id)
                .ToListAsync();

            roleIds.AddRange(permision.Roles.Select(r => r.Id));

            return roleIds;
        }

        /// <summary>
        /// Проверяет наличие доступа у ролей к ресурсу
        /// </summary>
        /// <param name="roleIds">Роли</param>
        /// <param name="tag">Ресурс</param>
        /// <returns>Список ролей с доступом</returns>
        private async Task<IEnumerable<Guid>> GetRolesWithAccess(IEnumerable<Guid> roleIds, string tag)
        {
            List<Guid> resultRoleIds = new();

            IEnumerable<Guid> lockPermissionIds = await _internalDataGetter
                .GetQueriable<ResourceTag>()
                .Where(t => t.Tag == tag)
                .Select(t => t.LockedPermissions.Select(p => p.Id))
                .SingleAsync();

            IEnumerable<Guid> permissionIds = await _internalDataGetter
                .GetQueriable<ResourceTag>()
                .Where(t => t.Tag == tag)
                .Select(t => t.Permissions.Select(p => p.Id))
                .SingleAsync();

            foreach (Guid roleId in roleIds)
            {
                bool? roleAccess = null;

                IEnumerable<Guid> rolePermissionids = await _internalDataGetter
                    .GetQueriable<Permission>()
                    .Where(p => p.Roles.Any(r => r.Id == roleId))
                    .Select(p => p.Id)
                    .ToListAsync();

                foreach (Guid permissionId in rolePermissionids)
                {
                    IEnumerable<Guid> motherIds = await _dataGetter.GetRecoursiveEntityIds<Permission>(permissionId, p => p.MotherPermissions);

                    if (lockPermissionIds.Contains(permissionId) || motherIds.Any(id => lockPermissionIds.Contains(id)))
                    {
                        roleAccess = false;
                    }
                    else if (!roleAccess.HasValue && (permissionIds.Contains(permissionId) || motherIds.Any(id => permissionIds.Contains(id))))
                    {
                        roleAccess = true;
                    }

                    if (roleAccess == false)
                    {
                        break;
                    }
                }

                if (roleAccess == true)
                {
                    resultRoleIds.Add(roleId);
                }
            }

            return resultRoleIds;
        }
    }
}
