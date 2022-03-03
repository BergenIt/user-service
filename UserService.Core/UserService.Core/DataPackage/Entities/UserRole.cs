using System;

using Microsoft.AspNetCore.Identity;

using UserService.Core.AuditPackage;

namespace UserService.Core.Entity
{
    [AuditEntity]
    public class UserRole : IdentityUserRole<Guid>, IBaseEntity
    {
        public Guid Id { get; set; }

        public Role Role { get; set; }

        public User User { get; set; }

        public string AuditName => Role?.AuditName ?? string.Empty;
    }
}

