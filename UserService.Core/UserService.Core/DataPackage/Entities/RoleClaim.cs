using System;

using Microsoft.AspNetCore.Identity;

using UserService.Core.AuditPackage;

namespace UserService.Core.Entity
{
    [AuditEntity]
    public class RoleClaim : IdentityRoleClaim<Guid>, IBaseEntity
    {
        public string AuditName => ClaimValue;
        public new Guid Id { get; set; } = Guid.NewGuid();

        public PermissionAssert PermissionAssert { get => Enum.Parse<PermissionAssert>(base.ClaimValue); set => base.ClaimValue = value.ToString(); }

        public Role Role { get; set; }
        public ResourceTag ResourceTag { get; set; }

    }
}

