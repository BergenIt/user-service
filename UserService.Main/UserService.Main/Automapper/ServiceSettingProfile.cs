
using AutoMapper;

using UserService.Core.ServiceSettings;
using UserService.Proto;

namespace UserService.Main.Automapper
{
    internal class ServiceSettingProfile : Profile
    {
        public ServiceSettingProfile()
        {
            _ = CreateMap<ServiceSetting, Core.Entity.ServiceSetting>()
                .ForMember(
                    o => o.ServiceSettingAttribute,
                    a => a.ConvertUsing<EnumReverseConverter<ServiceSettingAttribute>, string>()
                );

            _ = CreateMap<Core.Entity.ServiceSetting, ServiceSetting>()
                .ForMember(s => s.ServiceSettingValue, o => o.MapFrom(d => d.IsCredential() ? ServiceSettingManager.CredentialValue : d.ServiceSettingValue))
                .ForMember(
                    o => o.ServiceSettingAttribute,
                    a => a.ConvertUsing<EnumConverter<ServiceSettingAttribute>, ServiceSettingAttribute>()
                );
        }
    }
}
