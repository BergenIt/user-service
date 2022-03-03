using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension;

using Microsoft.EntityFrameworkCore;
using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Data.EntityWorkers
{
    public class RoleGetter : IRoleGetter
    {
        private readonly IInternalDataGetter _dataGetter;

        public RoleGetter(IInternalDataGetter dataGetter)
        {
            _dataGetter = dataGetter;
        }

        public Task<IEnumerable<RoleClaim>> GetRoleClaims(Guid roleId)
        {
            return _dataGetter
                .GetQueriable<RoleClaim>()
                .AsNoTracking()
                .Where(c => c.RoleId == roleId)
                .Include(r => r.ResourceTag)
                .ToListAsync()
                .ContinueWith(t => t.Result.AsEnumerable());
        }
    }
}
