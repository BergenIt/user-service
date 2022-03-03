using System;

using UserService.Core.AuditPackage;

namespace UserService.Core.Entity
{
    [AuditEntity]
    public class UserNotificationSetting : NotificationSetting
    {
        public override string AuditName => ContractProfile?.AuditName ?? User?.AuditName ?? string.Empty;

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}

