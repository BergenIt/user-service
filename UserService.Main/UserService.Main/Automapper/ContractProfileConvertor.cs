using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using UserService.Core.NotifyEventTypeGetter;
using UserService.Proto;

namespace UserService.Main.Automapper
{
    /// <summary>
    /// Конвертер сущности ContractProfile
    /// </summary>
    public class ContractProfileConvertor :
        ITypeConverter<Core.Entity.ContractProfile, ContractProfile>,
        ITypeConverter<ContractProfile, Core.Entity.ContractProfile>,
        ITypeConverter<ContractProfileCreateCommand, Core.Entity.ContractProfile>,
        ITypeConverter<ContractProfileUpdateCommand, Core.Entity.ContractProfile>
    {
        private readonly INotifyEventTypeGetter _notifyEventTypeGetter;

        /// <summary>
        /// Конвертер сущности ContractProfile
        /// </summary>
        /// <param name="notifyEventTypeGetter"></param>
        public ContractProfileConvertor(INotifyEventTypeGetter notifyEventTypeGetter)
        {
            _notifyEventTypeGetter = notifyEventTypeGetter;
        }

        /// <summary>
        /// Конвертер сущности ContractProfile
        /// </summary>
        /// <param name="contractProfileProto"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Core.Entity.ContractProfile Convert(ContractProfile contractProfileProto, Core.Entity.ContractProfile destination, ResolutionContext context)
        {
            Core.Entity.ContractProfile contractProfile = new()
            {
                Comment = contractProfileProto.Comment,
                Id = System.Guid.Parse(contractProfileProto.Id),
                Name = contractProfileProto.Id,
                NotifyEventType = contractProfileProto.NotifyEventType,

                ContractSettingLines = context.Mapper.Map<ICollection<Core.Entity.ContractSettingLine>>(contractProfileProto.ContractSettingLines),
            };

            contractProfile.NotifyEventType = _notifyEventTypeGetter.GetSourceNotifyEventType(contractProfile.NotifyEventType);

            IDictionary<string, string> translated = _notifyEventTypeGetter.GetNotifyEventTypePropperties(contractProfile.NotifyEventType);

            foreach (Core.Entity.ContractSettingLine contractSettingLine in contractProfile.ContractSettingLines)
            {
                Core.Entity.ContractSettingPropperty[] contractSettingPropperties = contractSettingLine
                    .ContractPropperties
                    .Select(p =>
                    {
                        p.ContractName = translated.Single(t => t.Value == p.ContractName).Key;
                        return p;
                    })
                    .ToArray();

                contractSettingLine.ContractPropperties = contractSettingPropperties;
            }

            return contractProfile;
        }

        /// <summary>
        /// Конвертер сущности ContractProfile
        /// </summary>
        /// <param name="contractProfileCore"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public ContractProfile Convert(Core.Entity.ContractProfile contractProfileCore, ContractProfile destination, ResolutionContext context)
        {
            IDictionary<string, string> translated = _notifyEventTypeGetter.GetNotifyEventTypePropperties(contractProfileCore.NotifyEventType);

            ContractProfile contractProfile = new()
            {
                Id = contractProfileCore.Id.ToString(),
                Comment = contractProfileCore.Comment,
                Name = contractProfileCore.Name,
                NotifyEventType = contractProfileCore.NotifyEventType,
                ContractSettingLines = { context.Mapper.Map<IEnumerable<ContractSettingLine>>(contractProfileCore.ContractSettingLines) },
            };

            contractProfile.NotifyEventType = _notifyEventTypeGetter.GetTranslatedNotifyEventType(contractProfileCore.NotifyEventType);

            foreach (ContractSettingLine contractSetting in contractProfile.ContractSettingLines)
            {
                ContractSettingPropperty[] translatedNames = contractSetting.ContractPropperties
                    .Select(p =>
                    {
                        p.ContractName = translated.SingleOrDefault(t => t.Key == p.ContractName).Value;
                        return p;
                    })
                    .Where(s => s.ContractName != null)
                    .ToArray();

                contractSetting.ContractPropperties.Clear();
                contractSetting.ContractPropperties.AddRange(translatedNames);
            }

            return contractProfile;
        }

        /// <summary>
        /// Конвертер сущности ContractProfile
        /// </summary>
        /// <param name="contractProfileCreate"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Core.Entity.ContractProfile Convert(ContractProfileCreateCommand contractProfileCreate, Core.Entity.ContractProfile destination, ResolutionContext context)
        {
            ContractProfile contractProfile = new()
            {
                Id = System.Guid.Empty.ToString(),
                Comment = contractProfileCreate.Comment,
                ContractSettingLines = { contractProfileCreate.ContractSettingLines },
                Name = contractProfileCreate.Name,
                NotifyEventType = contractProfileCreate.NotifyEventType
            };

            return Convert(contractProfile, destination, context);
        }

        /// <summary>
        /// Конвертер сущности ContractProfile
        /// </summary>
        /// <param name="contractProfileUpdate"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Core.Entity.ContractProfile Convert(ContractProfileUpdateCommand contractProfileUpdate, Core.Entity.ContractProfile destination, ResolutionContext context)
        {
            ContractProfile contractProfile = new()
            {
                Id = contractProfileUpdate.Id,
                Comment = contractProfileUpdate.Comment,
                ContractSettingLines = { contractProfileUpdate.ContractSettingLines },
                Name = contractProfileUpdate.Name,
                NotifyEventType = contractProfileUpdate.NotifyEventType
            };

            return Convert(contractProfile, destination, context);
        }
    }
}
