
using AutoMapper;

using UserService.Proto;

namespace UserService.Main.Automapper
{
    internal class PositionEntityProfile : Profile
    {
        public PositionEntityProfile()
        {
            _ = CreateMap<Position, Core.Entity.Position>().ReverseMap();
            _ = CreateMap<PositionCreateCommand, Core.Entity.Position>();
            _ = CreateMap<PositionUpdateCommand, Core.Entity.Position>();
        }
    }
}
