namespace UserService.Core.Entity
{
    public class ResourceTagRelation : BaseEntity
    {
        public ResourceTag ResourceTag { get; set; }
        public ResourceTag DependentResourceTag { get; set; }

        public string ResourceTagValue { get; set; }
        public string DependentResourceTagValue { get; set; }

        public PermissionAssert PermissionAssert { get; set; }

        public override string AuditName => ResourceTagValue;
    }
}
