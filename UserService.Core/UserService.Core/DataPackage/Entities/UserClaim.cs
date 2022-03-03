using System;

using Microsoft.AspNetCore.Identity;

namespace UserService.Core.Entity
{
    public class UserClaim : IdentityUserClaim<Guid>, IBaseEntity
    {
        public string AuditName { get => ClaimValue; }

        public new Guid Id { get; set; } = Guid.NewGuid();

        public User User { get; set; }
    }
}

