using System;

namespace UserService.Core.NotificationPackage.UserNotificationGetter
{
    public record UserNotificationRecord(Guid Id, string NotifyEventType, DateTimeOffset Timestamp, string Message, string ObjectId, bool IsRead);
}
