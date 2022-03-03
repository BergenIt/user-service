using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    public interface IRoleGetter
    {
        Task<IEnumerable<RoleClaim>> GetRoleClaims(Guid roleId);
    }
}
