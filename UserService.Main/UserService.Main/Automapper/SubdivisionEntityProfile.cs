
using AutoMapper;

using UserService.Proto;

namespace UserService.Main.Automapper
{
    internal class SubdivisionEntityProfile : Profile
    {
        public SubdivisionEntityProfile()
        {
            _ = CreateMap<Subdivision, Core.Entity.Subdivision>().ReverseMap();
            _ = CreateMap<SubdivisionCreateCommand, Core.Entity.Subdivision>();
            _ = CreateMap<SubdivisionUpdateCommand, Core.Entity.Subdivision>();
        }
    }
}
