using AutoMapper;

using UserService.Proto;

namespace UserService.Main.Automapper
{
    internal class ContractSettingProfile : Profile
    {
        public ContractSettingProfile()
        {
            CreateMap<Core.Entity.ContractProfile, ContractProfile>().ConvertUsing<ContractProfileConvertor>();
            CreateMap<ContractProfile, Core.Entity.ContractProfile>().ConvertUsing<ContractProfileConvertor>();
            CreateMap<ContractProfileUpdateCommand, Core.Entity.ContractProfile>().ConvertUsing<ContractProfileConvertor>();
            CreateMap<ContractProfileCreateCommand, Core.Entity.ContractProfile>().ConvertUsing<ContractProfileConvertor>();

            _ = CreateMap<ContractSettingLine, Core.Entity.ContractSettingLine>();
            _ = CreateMap<Core.Entity.ContractSettingLine, ContractSettingLine>();

            _ = CreateMap<Core.Entity.ContractSettingPropperty, ContractSettingPropperty>();
            _ = CreateMap<ContractSettingPropperty, Core.Entity.ContractSettingPropperty>();
        }
    }
}
