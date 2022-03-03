using System;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using UserService.Core.Entity;
using UserService.Core.PolindromHasher;

namespace UserService.Data
{
    public class CacheContext : UserServiceContext
    {
        public CacheContext(IPasswordHasher passwordHasher, DbContextOptions<CacheContext> options)
            : base(passwordHasher, options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = modelBuilder.Ignore<RoleClaim>();
            _ = modelBuilder.Ignore<Permission>();
            _ = modelBuilder.Ignore<Position>();
            _ = modelBuilder.Ignore<ServiceSetting>();
            _ = modelBuilder.Ignore<ResourceTag>();
            _ = modelBuilder.Ignore<ResourceTagRelation>();

            _ = modelBuilder.Ignore<IdentityUserToken<Guid>>();
            _ = modelBuilder.Ignore<IdentityUserLogin<Guid>>();
            _ = modelBuilder.Ignore<IdentityUserClaim<Guid>>();

            _ = modelBuilder.Entity<Role>(b =>
            {
                _ = b.Ignore(r => r.Name);
                _ = b.Ignore(r => r.Comment);
                _ = b.Ignore(r => r.ConcurrencyStamp);
                _ = b.Ignore(r => r.NormalizedName);
                _ = b.Ignore(r => r.Permissions);
            });

            _ = modelBuilder.Entity<User>(b =>
            {
                _ = b.Ignore(u => u.AccessFailedCount);
                _ = b.Ignore(u => u.ConcurrencyStamp);
                _ = b.Ignore(u => u.Description);
                _ = b.Ignore(u => u.RequestNumber);
                _ = b.Ignore(u => u.EmailConfirmed);
                _ = b.Ignore(u => u.FullName);
                _ = b.Ignore(u => u.IsLdapUser);
                _ = b.Ignore(u => u.LastLogin);
                _ = b.Ignore(u => u.LockoutEnabled);
                _ = b.Ignore(u => u.LockoutEnd);
                _ = b.Ignore(u => u.NormalizedEmail);
                _ = b.Ignore(u => u.NormalizedUserName);
                _ = b.Ignore(u => u.PasswordHash);
                _ = b.Ignore(u => u.PhoneNumber);
                _ = b.Ignore(u => u.PhoneNumberConfirmed);
                _ = b.Ignore(u => u.Position);
                _ = b.Ignore(u => u.PositionId);
                _ = b.Ignore(u => u.SecurityStamp);
                _ = b.Ignore(u => u.TwoFactorEnabled);
            });

            _ = modelBuilder.Entity<Subdivision>(b =>
            {
                _ = b.Ignore(s => s.Name);
                _ = b.Ignore(s => s.Comment);
            });

            _ = modelBuilder.Entity<WebHook>(b =>
            {
                _ = b.Ignore(s => s.Name);
                _ = b.Ignore(s => s.Comment);
            });

            _ = modelBuilder.Entity<ContractProfile>(b =>
            {
                _ = b.Ignore(s => s.Name);
                _ = b.Ignore(s => s.Comment);
            });
        }
    }
}
