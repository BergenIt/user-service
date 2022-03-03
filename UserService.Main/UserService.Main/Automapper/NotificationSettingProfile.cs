using AutoMapper;

using UserService.Proto;

namespace UserService.Main.Automapper
{
    internal class NotificationSettingProfile : Profile
    {
        public NotificationSettingProfile()
        {
            CreateMap<RoleNotifySetting, Core.Entity.RoleNotificationSetting>().ConvertUsing<RoleNotificationSettingConvertor>();
            CreateMap<RoleNotifySettingUpdateCommand, Core.Entity.RoleNotificationSetting>().ConvertUsing<RoleNotificationSettingConvertor>();
            CreateMap<RoleNotifySettingCreateCommand, Core.Entity.RoleNotificationSetting>().ConvertUsing<RoleNotificationSettingConvertor>();
            CreateMap<Core.Entity.RoleNotificationSetting, RoleNotifySetting>().ConvertUsing<RoleNotificationSettingConvertor>();

            CreateMap<UserNotifySetting, Core.Entity.UserNotificationSetting>().ConvertUsing<UserNotificationSettingConvertor>();
            CreateMap<UserNotifySettingUpdateCommand, Core.Entity.UserNotificationSetting>().ConvertUsing<UserNotificationSettingConvertor>();
            CreateMap<UserNotifySettingCreateCommand, Core.Entity.UserNotificationSetting>().ConvertUsing<UserNotificationSettingConvertor>();
            CreateMap<Core.Entity.UserNotificationSetting, UserNotifySetting>().ConvertUsing<UserNotificationSettingConvertor>();
        }
    }
}
