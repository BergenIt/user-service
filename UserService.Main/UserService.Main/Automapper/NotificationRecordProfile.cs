
using AutoMapper;

using UserService.Core.NotificationPackage.UserNotificationGetter;
using UserService.Proto;

namespace UserService.Main.Automapper
{
    internal class NotificationRecordProfile : Profile
    {
        public NotificationRecordProfile()
        {
            _ = CreateMap<UserNotificationRecord, Notification>()
                .ForMember(
                    o => o.NotifyEventType,
                    a => a.ConvertUsing<NotifyEventTypeConverter, string>()
                );
        }
    }
}
