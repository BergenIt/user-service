using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

using UserService.Core.AuditPackage;

namespace UserService.Core.Entity
{
    [AuditEntity]
    public class Role : IdentityRole<Guid>, IBaseEntity
    {
        public string Comment { get; set; }

        public DateTime RoleExpiration { get; set; }

        public ICollection<Permission> Permissions { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RoleNotificationSetting> RoleNotificationSettings { get; set; }

        public ICollection<RoleClaim> RoleClaims { get; set; }

        public string AuditName => Name;
    }
}

