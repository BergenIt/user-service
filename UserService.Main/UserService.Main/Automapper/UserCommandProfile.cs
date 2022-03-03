
using AutoMapper;

using UserService.Proto;

namespace UserService.Main.Automapper
{
    internal class UserCommandProfile : Profile
    {
        public UserCommandProfile() : base()
        {
            CreateMap<UserCreateCommand, Core.UserManager.UserCreateCommand>().ConvertUsing<UserCommandCoverter>();
            CreateMap<UserUpdateCommand, Core.UserManager.UserUpdateCommand>().ConvertUsing<UserCommandCoverter>();
        }
    }
}
