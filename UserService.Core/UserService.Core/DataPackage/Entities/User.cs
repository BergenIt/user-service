using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

using UserService.Core.AuditPackage;

namespace UserService.Core.Entity
{
    [AuditEntity]
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
            UserClaims = new HashSet<UserClaim>();
            UserNotificationSettings = new HashSet<UserNotificationSetting>();
        }

        public string AuditName => UserName;

        public string FullName { get; set; }
        public string Description { get; set; }
        public string RequestNumber { get; set; }
        public DateTimeOffset? LastLogin { get; set; }

        public DateTimeOffset UserExpiration { get; set; }
        public DateTimeOffset PasswordExpiration { get; set; }

        public bool UserLock { get; set; }

        public Guid? PositionId { get; set; }
        public Position Position { get; set; }

        public Guid SubdivisionId { get; set; }
        public Subdivision Subdivision { get; set; }

        public ICollection<UserClaim> UserClaims { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public ICollection<UserNotificationSetting> UserNotificationSettings { get; set; }

        public bool IsLdapUser => string.IsNullOrWhiteSpace(PasswordHash);

        public UserState UserState
        {
            get
            {
                if (UserLock)
                {
                    return UserState.Lock;
                }

                if (UserExpiration < DateTime.UtcNow || PasswordExpiration < DateTime.UtcNow)
                {
                    return UserState.Unactive;
                }

                return UserState.Active;
            }
        }

        public DateTimeOffset RegistredDate { get; set; }
    }
}

