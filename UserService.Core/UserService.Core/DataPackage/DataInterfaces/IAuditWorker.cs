using System.Collections.Generic;
using System;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.Entity;
using UserService.Core.Models;
using System.Linq;

namespace UserService.Core.DataInterfaces
{
    /// <summary>
    /// Организует работу с аудитом
    /// </summary>
    public interface IAuditWorker
    {
        /// <summary>
        /// Получить страницу записей аудита
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IPageItems<Audit>> GetAudits(FilterContract filter);

        Task<IPageItems<SystemAuditRecord>> GetSystemAudits(FilterContract filter);
        Task<IPageItems<IGrouping<string, SubdivisionAuditRecord>>> GetSubdivisionAudits(IEnumerable<Guid> subdivisionIds, FilterContract filter);
        Task<IPageItems<UserAuditRecord>> GetUserAudits(IEnumerable<string> userNames, IEnumerable<Guid> subdivisionIds, FilterContract filter);

        /// <summary>
        /// Создать запись в журнале аудита
        /// </summary>
        /// <param name="auditCreateCommand"></param>
        /// <returns></returns>
        Task CreateAudit(AuditCreateCommand auditCreateCommand);

        /// <summary>
        /// Создать запись в журнале аудита
        /// </summary>
        /// <param name="auditCreateCommand"></param>
        /// <returns></returns>
        Task CreateAudit(string ip, AuditCreateCommand auditCreateCommand);
    }
}
