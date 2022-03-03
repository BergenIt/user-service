using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using DatabaseExtension;

using Microsoft.EntityFrameworkCore;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Data.EntityWorkers
{
    public class PermissionGetter : IPermissionGetter
    {
        private readonly IInternalDataGetter _dataGetter;

        public PermissionGetter(IInternalDataGetter dataGetter)
        {
            _dataGetter = dataGetter;
        }

        public Task<IPageItems<Permission>> GetPermissionPage(FilterContract filterContract) => _dataGetter
            .GetPage<Permission>(filterContract);

        public Task<Permission> GetPermission(Guid id) => _dataGetter
            .GetQueriable<Permission>()
            .AsNoTracking()
            .Include(p => p.LockedResourceTags)
            .Include(p => p.ResourceTags)
            .Include(p => p.MotherPermissions)
            .Include(p => p.Roles)
            .SingleAsync(p => p.Id == id);

        public Task<IEnumerable<Permission>> GetRolePermissions(Guid roleId) => _dataGetter
            .GetQueriable<Permission>()
            .AsNoTracking()
            .Include(p => p.LockedResourceTags)
            .Include(p => p.ResourceTags)
            .Include(p => p.MotherPermissions)
            .Where(p => p.Roles.Any(r => r.Id == roleId))
            .ToListAsync()
            .ContinueWith(t => t.Result.AsEnumerable());

        public async Task<IEnumerable<ResourceTag>> GetRoleResources(Guid roleId)
        {
            IEnumerable<Guid> permissionIds = await GetAllPermissionIds(roleId);

            IQueryable<Permission> permissionQuery = _dataGetter
                .GetQueriable<Permission>()
                .Where(p => permissionIds.Contains(p.Id));

            string[] tags = await permissionQuery
                .SelectMany(p => p.ResourceTags.Select(t => t.Tag))
                .ToArrayAsync();

            string[] lockedResourceTags = await permissionQuery
                .SelectMany(p => p.LockedResourceTags.Select(t => t.Tag))
                .ToArrayAsync();

            List<string> resources = tags
                .Where(t => !lockedResourceTags.Contains(t))
                .Distinct()
                .ToList();

            List<ResourceTag> resourceTags = await _dataGetter
                .GetQueriable<ResourceTag>()
                .Where(t => resources.Contains(t.Tag))
                .ToListAsync();

            return resourceTags;
        }

        public async Task<IEnumerable<Claim>> GetRoleAccess(Guid roleId)
        {
            IEnumerable<Guid> permissionIds = await GetAllPermissionIds(roleId);

            IQueryable<Permission> permissionQuery = _dataGetter
                .GetQueriable<Permission>()
                .Where(p => permissionIds.Contains(p.Id));

            string[] resourceTags = await permissionQuery
                .SelectMany(p => p.ResourceTags.Select(t => t.Tag))
                .ToArrayAsync();

            string[] lockedResourceTags = await permissionQuery
                .SelectMany(p => p.LockedResourceTags.Select(t => t.Tag))
                .ToArrayAsync();

            List<string> resources = resourceTags
                .Where(t => !lockedResourceTags.Contains(t))
                .Distinct()
                .ToList();

            IDictionary<string, PermissionAssert> claims = new ConcurrentDictionary<string, PermissionAssert>();

            List<RoleClaim> roleClaims = await _dataGetter
                .GetQueriable<RoleClaim>()
                .Where(c => c.RoleId == roleId && resources.Contains(c.ClaimType))
                .Select(c => new RoleClaim
                {
                    ClaimValue = c.ClaimValue,
                    ClaimType = c.ClaimType,
                })
                .ToListAsync();

            foreach (RoleClaim roleClaim in roleClaims)
            {
                IDictionary<string, PermissionAssert> tags = await GetRelationsTags(new string[] { roleClaim.ClaimType });

                TryAddClaim(claims, roleClaim.ClaimType, roleClaim.PermissionAssert);

                while (tags.Any())
                {
                    foreach (KeyValuePair<string, PermissionAssert> tag in tags)
                    {
                        TryAddClaim(claims, tag.Key, tag.Value);
                    }

                    tags = await GetRelationsTags(tags.Keys);
                }
            }

            return claims.Select(c => new Claim(c.Key, c.Value.ToString()));
        }

        private static void TryAddClaim(IDictionary<string, PermissionAssert> claims, string tag, PermissionAssert assert)
        {
            if (!claims.TryGetValue(tag, out PermissionAssert permissionAssert))
            {
                claims.Add(tag, assert);
            }
            else if (assert > permissionAssert)
            {
                _ = claims.Remove(tag);
                claims.Add(tag, assert);
            }
        }

        private Task<Dictionary<string, PermissionAssert>> GetRelationsTags(IEnumerable<string> resources) => _dataGetter
            .GetQueriable<ResourceTag>()
            .Where(t => resources.Contains(t.Tag))
            .SelectMany(p => p.ResourceTagRelations)
            .ToDictionaryAsync(
                k => k.ResourceTagValue,
                v => v.PermissionAssert
            );

        private async Task<IEnumerable<Guid>> GetAllPermissionIds(Guid roleId)
        {
            List<Guid> commonPermissionIds = new();

            IEnumerable<Guid> permissionIds = await _dataGetter
                .GetQueriable<Permission>()
                .Where(t => t.Roles.Any(c => c.Id == roleId))
                .Select(p => p.Id)
                .ToListAsync();

            commonPermissionIds.AddRange(permissionIds);

            IEnumerable<Guid> permissionMotherIds = await GetPermissionMothers(commonPermissionIds, permissionIds);

            while (permissionMotherIds.Any())
            {
                commonPermissionIds.AddRange(permissionMotherIds);

                permissionMotherIds = await GetPermissionMothers(commonPermissionIds, permissionMotherIds);
            }

            return commonPermissionIds;
        }

        private Task<IEnumerable<Guid>> GetPermissionMothers(IEnumerable<Guid> commonPermissionIds, IEnumerable<Guid> permissionIds) => _dataGetter
            .GetQueriable<Permission>()
            .Where(p => permissionIds.Contains(p.Id) && !commonPermissionIds.Contains(p.Id))
            .SelectMany(p => p.MotherPermissions.Select(m => m.Id))
            .ToListAsync()
            .ContinueWith(t => t.Result.AsEnumerable());

        public Task<IEnumerable<ResourceTag>> GetSystemResources() => _dataGetter
            .GetQueriable<ResourceTag>()
            .AsNoTracking()
            .Include(t => t.ResourceTagRelations)
            .ToListAsync()
            .ContinueWith(t => t.Result.AsEnumerable());
    }
}
