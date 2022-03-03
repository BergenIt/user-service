
using AutoMapper;

using UserService.Proto;

namespace UserService.Main.Automapper
{
    internal class UserEntityProfile : Profile
    {
        public UserEntityProfile()
        {
            CreateMap<Core.Entity.UserRole, Role>()
                .ConvertUsing(u => new()
                {
                    Id = u.RoleId.ToString(),
                    Name = u.Role == null ? string.Empty : u.Role.Name,
                    Comment = u.Role == null ? string.Empty : u.Role.Comment
                });

            _ = CreateMap<User, Core.Entity.User>()
                .ForMember(
                    o => o.UserState,
                    a => a.ConvertUsing<EnumReverseConverter<Core.Entity.UserState>, string>()
                );

            _ = CreateMap<Core.Entity.User, User>()
                .ForMember(u => u.Roles, o => o.MapFrom(s => s.UserRoles))
                .ForMember(
                    o => o.UserState,
                    a => a.ConvertUsing<EnumConverter<Core.Entity.UserState>, Core.Entity.UserState>()
                );
        }
    }
}
