using System.Collections.Generic;

using UserService.Core.AuditPackage;

namespace UserService.Core.Entity
{
    [AuditEntity]
    public class Subdivision : BaseEntity
    {
        public Subdivision()
        {
            Users = new HashSet<User>();
        }

        public override string AuditName => Name;

        public string Name { get; set; }
        public string Comment { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<RoleNotificationSetting> RoleNotificationSettings { get; set; }
    }

}

