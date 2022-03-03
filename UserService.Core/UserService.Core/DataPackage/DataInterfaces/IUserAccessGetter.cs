using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UserService.Core.DataInterfaces
{
    public interface IUserAccessGetter
    {
        Task<ICollection<Claim>> GetUserClaims(Guid userId);
    }
}
