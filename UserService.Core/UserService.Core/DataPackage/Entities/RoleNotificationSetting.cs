using System;

using UserService.Core.AuditPackage;

namespace UserService.Core.Entity
{
    [AuditEntity]
    public class RoleNotificationSetting : NotificationSetting
    {
        public override string AuditName => ContractProfile?.AuditName ?? Role?.AuditName ?? Subdivision?.AuditName ?? string.Empty;

        public Guid SubdivisionId { get; set; }
        public Guid RoleId { get; set; }

        public Subdivision Subdivision { get; set; }
        public Role Role { get; set; }
    }
}

