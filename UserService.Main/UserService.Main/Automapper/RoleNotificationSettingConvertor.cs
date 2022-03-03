using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using DatabaseExtension.Translator;

using UserService.Proto;

namespace UserService.Main.Automapper
{
    /// <summary>
    /// Конвертер сущности NotificationSetting
    /// </summary>
    public class RoleNotificationSettingConvertor :
        ITypeConverter<Core.Entity.RoleNotificationSetting, RoleNotifySetting>,
        ITypeConverter<RoleNotifySetting, Core.Entity.RoleNotificationSetting>,
        ITypeConverter<RoleNotifySettingCreateCommand, Core.Entity.RoleNotificationSetting>,
        ITypeConverter<RoleNotifySettingUpdateCommand, Core.Entity.RoleNotificationSetting>
    {
        private readonly ITranslator _translator;

        /// <summary>
        /// Конвертер сущности NotificationSetting
        /// </summary>
        /// <param name="translator"></param>
        public RoleNotificationSettingConvertor(ITranslator translator)
        {
            _translator = translator;
        }

        /// <summary>
        /// Конвертер сущности NotificationSetting
        /// </summary>
        /// <param name="notificationSetting"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public RoleNotifySetting Convert(Core.Entity.RoleNotificationSetting notificationSetting, RoleNotifySetting destination, ResolutionContext context)
        {
            IDictionary<Core.Entity.TargetNotify, string> targetNoties = _translator.GetEnumText<Core.Entity.TargetNotify>();

            Core.Entity.TargetNotify[] targets = notificationSetting.TargetNotifies.ToArray();

            notificationSetting.ContractProfile = null;

            RoleNotifySetting protoNotifySetting = new()
            {
                ContractProfileId = notificationSetting.ContractProfileId.ToString(),
                Enable = notificationSetting.Enable,
                Id = notificationSetting.Id.ToString(),
                RoleId = notificationSetting.RoleId.ToString(),
                SubdivisionId = notificationSetting.SubdivisionId.ToString(),
                TargetNotifies = { targetNoties.Where(e => targets.Contains(e.Key)).Select(e => e.Value) }
            };

            return protoNotifySetting;
        }

        /// <summary>
        /// Конвертер сущности NotificationSetting
        /// </summary>
        /// <param name="notificationSetting"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Core.Entity.RoleNotificationSetting Convert(RoleNotifySetting notificationSetting, Core.Entity.RoleNotificationSetting destination, ResolutionContext context)
        {
            IDictionary<Core.Entity.TargetNotify, string> targetNoties = _translator.GetEnumText<Core.Entity.TargetNotify>();

            Core.Entity.RoleNotificationSetting notification = new()
            {
                Id = Guid.Parse(notificationSetting.Id),
                Enable = notificationSetting.Enable,
                ContractProfileId = Guid.Parse(notificationSetting.ContractProfileId),
                RoleId = Guid.Parse(notificationSetting.RoleId),
                SubdivisionId = Guid.Parse(notificationSetting.SubdivisionId),
                TargetNotifies = targetNoties.Where(x => notificationSetting.TargetNotifies.Contains(x.Value)).Select(s => s.Key),
            };

            return notification;
        }

        /// <summary>
        /// Конвертер сущности NotificationSetting
        /// </summary>
        /// <param name="settingCreateCommand"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Core.Entity.RoleNotificationSetting Convert(RoleNotifySettingCreateCommand settingCreateCommand, Core.Entity.RoleNotificationSetting destination, ResolutionContext context)
        {
            RoleNotifySetting notifySetting = new RoleNotifySetting
            {
                Id = Guid.NewGuid().ToString(),
                ContractProfileId = settingCreateCommand.ContractProfileId,
                Enable = settingCreateCommand.Enable,
                RoleId = settingCreateCommand.RoleId,
                SubdivisionId = settingCreateCommand.SubdivisionId,
                TargetNotifies = { settingCreateCommand.TargetNotifies },
            };

            return Convert(notifySetting, destination, context);
        }

        /// <summary>
        /// Конвертер сущности NotificationSetting
        /// </summary>
        /// <param name="settingUpdateCommand"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Core.Entity.RoleNotificationSetting Convert(RoleNotifySettingUpdateCommand settingUpdateCommand, Core.Entity.RoleNotificationSetting destination, ResolutionContext context)
        {
            RoleNotifySetting notifySetting = new()
            {
                Id = settingUpdateCommand.Id,
                ContractProfileId = Guid.Empty.ToString(),
                Enable = settingUpdateCommand.Enable,
                RoleId = settingUpdateCommand.RoleId,
                SubdivisionId = settingUpdateCommand.SubdivisionId,
                TargetNotifies = { settingUpdateCommand.TargetNotifies },
            };

            return Convert(notifySetting, destination, context);
        }
    }
}
