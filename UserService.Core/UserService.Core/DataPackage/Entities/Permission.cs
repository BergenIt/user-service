using System.Collections.Generic;

using UserService.Core.AuditPackage;

namespace UserService.Core.Entity
{
    [AuditEntity]
    public class Permission : BaseEntity
    {
        public Permission()
        {
            ResourceTags = new HashSet<ResourceTag>();
            LockedResourceTags = new HashSet<ResourceTag>();

            MotherPermissions = new HashSet<Permission>();
            ChildPermissions = new HashSet<Permission>();

            Roles = new HashSet<Role>();
        }

        public string Name { get; set; }
        public string Comment { get; set; }

        public ICollection<Role> Roles { get; set; }

        public ICollection<ResourceTag> ResourceTags { get; set; }
        public ICollection<ResourceTag> LockedResourceTags { get; set; }

        public ICollection<Permission> MotherPermissions { get; set; }
        public ICollection<Permission> ChildPermissions { get; set; }

        public override string AuditName => Name;
    }
}
