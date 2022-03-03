using System.Collections.Generic;

namespace UserService.Core.Entity
{
    public class ResourceTag : BaseEntity
    {
        public const string AccessObjectIds = "AccessObjectIds";

        public string Tag { get; set; }
        public string Name { get; set; }

        public ICollection<Permission> Permissions { get; set; }
        public ICollection<Permission> LockedPermissions { get; set; }

        public ICollection<ResourceTagRelation> ResourceTagRelations { get; set; }
        public ICollection<ResourceTagRelation> DependentResourceTagRelations { get; set; }

        public ICollection<RoleClaim> RoleClaims { get; set; }

        public override string AuditName => Name;
    }
}
