
using System.Linq;

using AutoMapper;

using UserService.Proto;

namespace UserService.Main.Automapper
{
    internal class AuditEntityProfile : Profile
    {
        public AuditEntityProfile()
        {
            _ = CreateMap<Core.Entity.Audit, Audit>()
                .ForMember(a => a.Roles, o => o.MapFrom(s => s.Roles.Where(r => r != null)));

            _ = CreateMap<Core.Models.UserAuditRecord, UserAudit>();
            _ = CreateMap<Core.Models.SubdivisionAuditRecord, SubdivisionAudit>();
            _ = CreateMap<Core.Models.SystemAuditRecord, SystemAudit>();

            _ = CreateMap<IGrouping<string, Core.Models.SubdivisionAuditRecord>, SubdivisionAuditGroup>()
                .ForMember(s => s.Subdivision, o => o.MapFrom(d => d.Key))
                .ForMember(s => s.SubdivisionAuditLogsList, o => o.MapFrom(d => d.AsEnumerable()));

            _ = CreateMap<AuditCreateCommand, Core.Models.AuditCreateCommand>();
        }
    }
}
