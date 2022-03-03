
using AutoMapper;

using UserService.Proto;

namespace UserService.Main.Automapper
{
    internal class LifecycleIndicesProfile : Profile
    {
        public LifecycleIndicesProfile()
        {
            _ = CreateMap<LifecycleIndex, Core.Models.PolicyPhases>().ReverseMap();

            _ = CreateMap<Core.Models.HotPolicyPhase, HotPolicyPhase>();
            _ = CreateMap<Core.Models.DeletePolicyPhase, DeletePolicyPhase>();
            _ = CreateMap<Core.Models.WarmPolicyPhase, WarmPolicyPhase>();

            CreateMap<DeletePolicyPhase, Core.Models.DeletePolicyPhase>()
                .ConvertUsing(s => new Core.Models.DeletePolicyPhase(s.MinimumAge.ToTimeSpan()));

            CreateMap<WarmPolicyPhase, Core.Models.WarmPolicyPhase>()
                .ConvertUsing(s => new Core.Models.WarmPolicyPhase(s.MinimumAge.ToTimeSpan()));

            CreateMap<HotPolicyPhase, Core.Models.HotPolicyPhase>()
                .ConvertUsing(s => new Core.Models.HotPolicyPhase(s.MaximumAge.ToTimeSpan(), s.MaximumSize, s.MaximumDocuments));

        }
    }
}
