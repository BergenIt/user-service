using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using DatabaseExtension;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using UserService.Core.DataInterfaces;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    /// Сервис аудита
    /// </summary>
    public class AuditServices : AuditService.AuditServiceBase
    {
        private readonly IAuditWorker _auditWorker;
        private readonly IMapper _mapper;

        /// <summary>
        /// Сервис аудита
        /// </summary>
        /// <param name="auditWorker"></param>
        /// <param name="mapper"></param>
        public AuditServices(IAuditWorker auditWorker, IMapper mapper)
        {
            _auditWorker = auditWorker;
            _mapper = mapper;
        }

        /// <summary>
        /// Создать новую запись в журнале аудита
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Empty> CreateAudit(AuditCreateCommand request, ServerCallContext context)
        {
            Core.Models.AuditCreateCommand command = _mapper.Map<Core.Models.AuditCreateCommand>(request);

            await _auditWorker.CreateAudit(request.IpAddress, command);

            return new();
        }

        /// <summary>
        /// Получить логи аудита
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AuditPage> GetAuditLogs(GetAuditLogsRequest request, ServerCallContext context)
        {
            FilterContract filter = request.Filter.FromProtoFilter<Audit, Core.Entity.Audit>();

            IPageItems<Core.Entity.Audit> audits = await _auditWorker.GetAudits(filter);

            return new()
            {
                AuditLogsList = { _mapper.Map<IEnumerable<Audit>>(audits.Items) },
                CountItems = audits.CountItems
            };
        }

        /// <summary>
        /// Получитть отчет аудита по юзерам
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserAuditPage> GetUserAuditLogs(GetUserAuditRequest request, ServerCallContext context)
        {
            FilterContract filterContract = request.Filter.FromProtoFilter<Audit, Core.Entity.Audit>();

            IEnumerable<Guid> subdivisionIds = request.SubdivisionIds.Select(id => Guid.Parse(id));

            IPageItems<Core.Models.UserAuditRecord> pageItems = await _auditWorker.GetUserAudits(request.UserNames, subdivisionIds, filterContract);

            return new()
            {
                UserAuditLogsList = { _mapper.Map<IEnumerable<UserAudit>>(pageItems.Items) },
                CountItems = pageItems.CountItems,
            };
        }

        /// <summary>
        /// Получить отчет аудита по структурным подразделениям 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<SubdivisionAuditPage> GetSubdivisionAuditLogs(GetSubdivisionAuditRequest request, ServerCallContext context)
        {
            FilterContract filterContract = request.Filter.FromProtoFilter<Audit, Core.Entity.Audit>();

            IEnumerable<Guid> subdivisionIds = request.SubdivisionIds.Select(id => Guid.Parse(id));

            IPageItems<IGrouping<string, Core.Models.SubdivisionAuditRecord>> pageItems = await _auditWorker.GetSubdivisionAudits(subdivisionIds, filterContract);

            return new()
            {
                SubdivisionAuditLogsList = { _mapper.Map<IEnumerable<SubdivisionAuditGroup>>(pageItems.Items) },
                CountItems = pageItems.CountItems,
            };
        }

        /// <summary>
        /// Получит отчет аудита по системе
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<SystemAuditPage> GetSystemAuditLogs(GetSystemAuditRequest request, ServerCallContext context)
        {
            FilterContract filterContract = request.Filter.FromProtoFilter<Audit, Core.Entity.Audit>();

            IPageItems<Core.Models.SystemAuditRecord> pageItems = await _auditWorker.GetSystemAudits(filterContract);

            return new()
            {
                SystemAuditLogsList = { _mapper.Map<IEnumerable<SystemAudit>>(pageItems.Items) },
                CountItems = pageItems.CountItems,
            };
        }
    }
}
