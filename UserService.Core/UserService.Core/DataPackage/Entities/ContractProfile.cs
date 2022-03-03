using System.Collections.Generic;

using UserService.Core.AuditPackage;

namespace UserService.Core.Entity
{
    [AuditEntity]
    public class ContractProfile : BaseEntity
    {
        public ContractProfile()
        {
            ContractSettingLines = new HashSet<ContractSettingLine>();
            WebHooks = new HashSet<WebHook>();

            RoleNotificationSettings = new HashSet<RoleNotificationSetting>();
            UserNotificationSettings = new HashSet<UserNotificationSetting>();
        }

        public override string AuditName => Name;

        public string Name { get; set; }
        public string Comment { get; set; }

        public string NotifyEventType { get; set; }

        public ICollection<ContractSettingLine> ContractSettingLines { get; set; }

        public ICollection<RoleNotificationSetting> RoleNotificationSettings { get; set; }
        public ICollection<UserNotificationSetting> UserNotificationSettings { get; set; }

        public ICollection<WebHook> WebHooks { get; set; }
    }
}

