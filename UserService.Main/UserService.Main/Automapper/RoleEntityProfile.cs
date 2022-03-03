
using AutoMapper;

using UserService.Proto;

namespace UserService.Main.Automapper
{
    internal class RoleEntityProfile : Profile
    {
        public RoleEntityProfile()
        {
            _ = CreateMap<Role, Core.Entity.Role>().ReverseMap();
            _ = CreateMap<RoleCreateCommand, Core.Entity.Role>();
            _ = CreateMap<RoleUpdateCommand, Core.Entity.Role>();

            _ = CreateMap<RoleLevelAccess, Core.Entity.RoleClaim>()
                .ForMember(c => c.ResourceTag, o => o.Ignore())
                .ForMember(c => c.ClaimType, o => o.MapFrom(s => s.ResourceTag))
                .ForMember(
                    o => o.PermissionAssert,
                    a => a.ConvertUsing<EnumReverseConverter<Core.Entity.PermissionAssert>, string>()
                );

            _ = CreateMap<Core.Entity.RoleClaim, RoleLevelAccess>()
                .ForMember(c => c.ResourceTag, o => o.MapFrom(s => s.ClaimType))
                .ForMember(
                    o => o.PermissionAssert,
                    a => a.ConvertUsing<EnumConverter<Core.Entity.PermissionAssert>, Core.Entity.PermissionAssert>()
                )
                .ForMember(
                    o => o.Name,
                    a => a.MapFrom(c => c.ResourceTag.Name)
                );
        }
    }
}
