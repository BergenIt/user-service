using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core.DataPackage.DataWorkers
{
    public class RoleManager : IRoleManager
    {
        private readonly IDataWorker _dataWorker;
        private readonly IDataGetter _dataGetter;

        public RoleManager(IDataWorker dataWorker, IDataGetter dataGetter)
        {
            _dataGetter = dataGetter;
            _dataWorker = dataWorker;
        }

        public Task<Role> GetRole(Guid id)
        {
            return _dataGetter.GetSingleEntityAsync<Role>(id);
        }

        public Task<IPageItems<Role>> GetRoles(FilterContract filter)
        {
            return _dataGetter.GetPage<Role>(filter);
        }

        public async Task<IEnumerable<Role>> AddRoles(IEnumerable<Role> roles)
        {
            await _dataWorker.AddRangeAsync(roles);

            await _dataWorker.SaveChangesAsync();

            return roles;
        }

        public async Task<IEnumerable<Role>> RemoveRoles(IEnumerable<Guid> roleIds)
        {
            IEnumerable<Role> roles = await _dataWorker.RemoveRangeAsync<Role>(roleIds);

            await _dataWorker.SaveChangesAsync();

            return roles;
        }

        public async Task<IEnumerable<Role>> UpdateRoles(IEnumerable<Role> roles)
        {
            foreach (Role role in roles)
            {
                Role updated = await _dataWorker.UpdateAsync<Role>(
                    role.Id,
                    r => r.Name = role.Name,
                           r => r.Comment = role.Comment,
                           r => r.RoleExpiration = role.RoleExpiration
                );
            }

            await _dataWorker.SaveChangesAsync();

            return roles;
        }
    }
}
