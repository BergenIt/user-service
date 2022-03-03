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
    public class UserNotificationSettingConvertor :
        ITypeConverter<Core.Entity.UserNotificationSetting, UserNotifySetting>,
        ITypeConverter<UserNotifySetting, Core.Entity.UserNotificationSetting>,
        ITypeConverter<UserNotifySettingCreateCommand, Core.Entity.UserNotificationSetting>,
        ITypeConverter<UserNotifySettingUpdateCommand, Core.Entity.UserNotificationSetting>
    {
        private readonly ITranslator _translator;

        /// <summary>
        /// Конвертер сущности NotificationSetting
        /// </summary>
        /// <param name="translator"></param>
        public UserNotificationSettingConvertor(ITranslator translator)
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
        public UserNotifySetting Convert(Core.Entity.UserNotificationSetting notificationSetting, UserNotifySetting destination, ResolutionContext context)
        {
            IDictionary<Core.Entity.TargetNotify, string> targetNoties = _translator.GetEnumText<Core.Entity.TargetNotify>();

            Core.Entity.TargetNotify[] targets = notificationSetting.TargetNotifies.ToArray();

            notificationSetting.ContractProfile = null;

            UserNotifySetting protoNotifySetting = new()
            {
                ContractProfileId = notificationSetting.ContractProfileId.ToString(),
                Enable = notificationSetting.Enable,
                Id = notificationSetting.Id.ToString(),
                UserId = notificationSetting.UserId.ToString(),
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
        public Core.Entity.UserNotificationSetting Convert(UserNotifySetting notificationSetting, Core.Entity.UserNotificationSetting destination, ResolutionContext context)
        {
            IDictionary<Core.Entity.TargetNotify, string> targetNoties = _translator.GetEnumText<Core.Entity.TargetNotify>();

            Core.Entity.UserNotificationSetting notification = new()
            {
                Id = Guid.Parse(notificationSetting.Id),
                Enable = notificationSetting.Enable,
                ContractProfileId = Guid.Parse(notificationSetting.ContractProfileId),
                UserId = Guid.Parse(notificationSetting.UserId),
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
        public Core.Entity.UserNotificationSetting Convert(UserNotifySettingCreateCommand settingCreateCommand, Core.Entity.UserNotificationSetting destination, ResolutionContext context)
        {
            UserNotifySetting notifySetting = new UserNotifySetting
            {
                Id = Guid.NewGuid().ToString(),
                ContractProfileId = settingCreateCommand.ContractProfileId,
                Enable = settingCreateCommand.Enable,
                UserId = settingCreateCommand.UserId,
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
        public Core.Entity.UserNotificationSetting Convert(UserNotifySettingUpdateCommand settingUpdateCommand, Core.Entity.UserNotificationSetting destination, ResolutionContext context)
        {
            UserNotifySetting notifySetting = new()
            {
                Id = settingUpdateCommand.Id,
                ContractProfileId = Guid.Empty.ToString(),
                Enable = settingUpdateCommand.Enable,
                UserId = settingUpdateCommand.UserId,
                TargetNotifies = { settingUpdateCommand.TargetNotifies },
            };

            return Convert(notifySetting, destination, context);
        }
    }
}
