
using AutoMapper;

namespace UserService.Main.Automapper
{
    internal class WebhookProfile : Profile
    {
        public WebhookProfile()
        {
            _ = CreateMap<Core.Entity.WebHook, Proto.Webhook>()
                .ForMember(
                    o => o.WebHookContractType,
                    a => a.ConvertUsing<EnumConverter<Core.Entity.WebHookContractType>, Core.Entity.WebHookContractType>()
                );

            _ = CreateMap<Proto.Webhook, Core.Entity.WebHook>()
                .ForMember(
                    o => o.WebHookContractType,
                    a => a.ConvertUsing<EnumReverseConverter<Core.Entity.WebHookContractType>, string>()
                );

            _ = CreateMap<Proto.WebhookCreateCommand, Core.Entity.WebHook>()
                .ForMember(
                    o => o.WebHookContractType,
                    a => a.ConvertUsing<EnumReverseConverter<Core.Entity.WebHookContractType>, string>()
                );

            _ = CreateMap<Proto.WebhookUpdateCommand, Core.Entity.WebHook>()
                .ForMember(
                    o => o.WebHookContractType,
                    a => a.ConvertUsing<EnumReverseConverter<Core.Entity.WebHookContractType>, string>()
                );
        }
    }
}
