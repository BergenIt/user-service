using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    /// <summary>
    /// Работа с клаймами ролей (уровнями доступа к ресурсам)
    /// </summary>
    public interface IRoleClaimManager
    {
        /// <summary>
        /// Получает клаймы роли
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IEnumerable<RoleClaim>> GetRoleClaims(Guid roleId);

        /// <summary>
        /// Изменяет уровень доступа роли к ресурсу на уровне клаймов
        /// </summary>
        /// <param name="roleClaims"></param>
        /// <returns></returns>
        Task<IEnumerable<RoleClaim>> ChangeAssertLevelRoleClaims(IEnumerable<RoleClaim> roleClaims);
    }
}
