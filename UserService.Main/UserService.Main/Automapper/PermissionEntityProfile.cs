using System.Linq;

using AutoMapper;

using UserService.Proto;

namespace UserService.Main.Automapper
{
    internal class PermissionEntityProfile : Profile
    {
        public PermissionEntityProfile()
        {
            _ = CreateMap<Core.Entity.ResourceTag, Resource>().ReverseMap();

            _ = CreateMap<Core.Entity.Permission, Permission>()
                .ForMember(o => o.LockedResources, a => a.MapFrom(s => s.LockedResourceTags))
                .ForMember(o => o.Resources, a => a.MapFrom(s => s.ResourceTags));

            _ = CreateMap<Permission, Core.Entity.Permission>()
                .ForMember(o => o.LockedResourceTags, a => a.MapFrom(s => s.LockedResources))
                .ForMember(o => o.ResourceTags, a => a.MapFrom(s => s.Resources));

            _ = CreateMap<PermissionCreateCommand, Core.Entity.Permission>()
                .ForMember(o => o.Roles, a => a.MapFrom(s => s.RoleIds.Select(id => new Role { Id = id })))
                .ForMember(o => o.MotherPermissions, a => a.MapFrom(s => s.MotherPermissionIds.Select(id => new Permission { Id = id })))
                .ForMember(o => o.LockedResourceTags, a => a.MapFrom(s => s.LockedResourceIds.Select(id => new Resource { Id = id })))
                .ForMember(o => o.ResourceTags, a => a.MapFrom(s => s.ResourceIds.Select(id => new Resource { Id = id })));

            _ = CreateMap<PermissionUpdateCommand, Core.Entity.Permission>()
                .ForMember(o => o.Roles, a => a.MapFrom(s => s.RoleIds.Select(id => new Role { Id = id })))
                .ForMember(o => o.MotherPermissions, a => a.MapFrom(s => s.MotherPermissionIds.Select(id => new Permission { Id = id })))
                .ForMember(o => o.LockedResourceTags, a => a.MapFrom(s => s.LockedResourceIds.Select(id => new Resource { Id = id })))
                .ForMember(o => o.ResourceTags, a => a.MapFrom(s => s.ResourceIds.Select(id => new Resource { Id = id })));
        }
    }
}
