using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using DatabaseExtension.Translator;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using UserService.Core;
using UserService.Core.DataInterfaces;
using UserService.Core.Entity;
using UserService.Core.Models;
using UserService.Core.NotifyEventTypeGetter;
using UserService.Core.PolindromHasher;

namespace UserService.Data
{
    public abstract class UserServiceContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, IdentityUserLogin<Guid>, RoleClaim, IdentityUserToken<Guid>>
    {
        private readonly ValueComparer<IEnumerable<TargetNotify>> _targetComparer = new(
            (a, b) => a.SequenceEqual(b),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.AsEnumerable()
        );

        private readonly ValueComparer<IEnumerable<string>> _stringComparer = new(
            (a, b) => a.SequenceEqual(b),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.AsEnumerable()
        );

        private const string DefaultPassword = "aA123123@";
        private const string DefaultUserName = "SuperAdmin";
        private const string DefaultEmail = "SuperAdmin@bergen.tech";

        private const string DefaultRole = "SuperAdmin";

        private const string DefaultSubdivision = "Стандартное подразделение";

        private readonly IPasswordHasher _passwordHasher;

        protected UserServiceContext(IPasswordHasher passwordHasher, DbContextOptions options) : base(options)
        {
            _passwordHasher = passwordHasher;
        }

        public DbSet<Subdivision> Subdivisions { get; set; }

        public DbSet<ContractProfile> ContractProfiles { get; set; }

        public DbSet<ContractSettingLine> ContractSettingLines { get; set; }
        public DbSet<ContractSettingPropperty> ContractPropperties { get; set; }

        public DbSet<RoleNotificationSetting> RoleNotificationSettings { get; set; }
        public DbSet<UserNotificationSetting> UserNotificationSettings { get; set; }

        public DbSet<WebHook> WebHooks { get; set; }

        public DbSet<NotifyEventType> NotifyEventTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = modelBuilder.Entity<UserClaim>()
                .Property(x => x.Id)
                .HasValueGenerator<GuidValueGenerator>()
                .ValueGeneratedOnAdd();

            _ = modelBuilder.Entity<RoleClaim>()
                .Property(x => x.Id)
                .HasValueGenerator<GuidValueGenerator>()
                .ValueGeneratedOnAdd();

            _ = modelBuilder.Entity<Audit>(b =>
            {
                _ = b.Ignore(a => a.IndexKey);

                b.Property(x => x.Roles)
                    .HasConversion(
                        t => string.Join(";", t.Select(e => e.ToString())),
                        t => t.Split(";", StringSplitOptions.RemoveEmptyEntries))
                    .Metadata
                    .SetValueComparer(_stringComparer);
            });

            _ = modelBuilder.Entity<UserClaim>(b =>
            {
                _ = b.Property(et => et.Id).ValueGeneratedNever();
                _ = b.HasKey(r => r.Id);
                _ = b.HasOne(r => r.User).WithMany(r => r.UserClaims).HasForeignKey(r => r.UserId).HasPrincipalKey(r => r.Id).OnDelete(DeleteBehavior.Cascade);
                _ = b.HasIndex(t => new { t.UserId, t.ClaimValue, t.ClaimType }).IsUnique();

                _ = b.HasData(new UserClaim()
                {
                    Id = CreateGuid(1),
                    ClaimType = ResourceTag.AccessObjectIds,
                    ClaimValue = CreateGuid(1).ToString(),
                    UserId = CreateGuid(1),
                });
            });

            _ = modelBuilder.Entity<NotifyEventType>(b =>
            {
                _ = b.HasIndex(r => r.Type).IsUnique();
                _ = b.HasMany(e => e.ContractProfiles).WithOne().HasForeignKey(e => e.NotifyEventType).HasPrincipalKey(e => e.Type).OnDelete(DeleteBehavior.Cascade);
            });

            _ = modelBuilder.Entity<Notification>(b =>
            {
                _ = b.Ignore(a => a.Relation);
                _ = b.Property(n => n.JsonData).HasColumnType("jsonb");
                _ = b.HasMany(n => n.UserNotifications).WithOne().HasForeignKey(n => n.NotificationId).HasPrincipalKey(n => n.Id).OnDelete(DeleteBehavior.Cascade);
            });

            _ = modelBuilder.Entity<UserNotification>(b =>
            {
                _ = b.Ignore(a => a.Relation);
            });

            _ = modelBuilder.Entity<User>(b =>
            {
                _ = b.Property(r => r.UserName).IsRequired();

                _ = b.HasIndex(u => u.UserName).IsUnique();
                _ = b.HasIndex(u => u.NormalizedUserName).IsUnique();
                _ = b.HasIndex(u => u.Email).IsUnique();
                _ = b.HasIndex(u => u.NormalizedEmail).IsUnique();

                _ = b.HasOne(u => u.Position).WithMany(r => r.Users).HasForeignKey(p => p.PositionId).HasPrincipalKey(u => u.Id).OnDelete(DeleteBehavior.Cascade);
                _ = b.HasOne(u => u.Subdivision).WithMany(r => r.Users).HasForeignKey(u => u.SubdivisionId).HasPrincipalKey(s => s.Id).OnDelete(DeleteBehavior.Cascade);
                _ = b.HasMany(u => u.UserRoles).WithOne(r => r.User).HasForeignKey(r => r.UserId).HasPrincipalKey(u => u.Id).OnDelete(DeleteBehavior.Cascade);

                _ = b.HasData(new User
                {
                    UserName = DefaultUserName,
                    Email = DefaultEmail,
                    PasswordHash = _passwordHasher.HashPassword(DefaultUserName, DefaultPassword),
                    PasswordExpiration = DateTime.MaxValue,
                    UserExpiration = DateTime.MaxValue,
                    NormalizedUserName = DefaultUserName.ToUpperInvariant(),
                    NormalizedEmail = DefaultEmail.ToUpperInvariant(),
                    SubdivisionId = CreateGuid(1),
                    Id = CreateGuid(1),
                    SecurityStamp = Guid.NewGuid().ToString()
                });
            });

            _ = modelBuilder.Entity<Role>(b =>
            {
                _ = b.Property(r => r.Name).IsRequired();
                _ = b.HasIndex(u => u.Name).IsUnique();
                _ = b.HasIndex(u => u.NormalizedName).IsUnique();
                _ = b.HasMany(u => u.UserRoles).WithOne(r => r.Role).HasForeignKey(r => r.RoleId).HasPrincipalKey(r => r.Id).OnDelete(DeleteBehavior.Cascade);
                _ = b.HasMany(u => u.Permissions).WithMany(r => r.Roles);
                _ = b.HasData(new Role
                {
                    Id = CreateGuid(1),
                    Name = DefaultRole,
                    NormalizedName = DefaultRole.ToUpperInvariant(),
                    RoleExpiration = DateTime.MaxValue,
                });
            });

            _ = modelBuilder.Entity<UserRole>(b =>
            {
                _ = b.HasKey(r => r.Id);
                _ = b.HasData(new UserRole
                {
                    Id = CreateGuid(1),
                    RoleId = CreateGuid(1),
                    UserId = CreateGuid(1),
                });
            });

            _ = modelBuilder.Entity<ContractProfile>(b =>
            {
                _ = b.HasMany(x => x.UserNotificationSettings).WithOne(s => s.ContractProfile).HasForeignKey(s => s.ContractProfileId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Cascade);
                _ = b.HasMany(x => x.RoleNotificationSettings).WithOne(s => s.ContractProfile).HasForeignKey(s => s.ContractProfileId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Cascade);
                _ = b.HasMany(x => x.WebHooks).WithOne(s => s.ContractProfile).HasForeignKey(s => s.ContractProfileId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Cascade);
                _ = b.HasMany(x => x.ContractSettingLines).WithOne(s => s.ContractProfile).HasForeignKey(s => s.ContractProfileId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Cascade);
            });

            _ = modelBuilder.Entity<RoleNotificationSetting>(b =>
            {
                _ = b.HasKey(x => x.Id);
                _ = b.HasOne(x => x.Subdivision).WithMany(s => s.RoleNotificationSettings).HasForeignKey(s => s.SubdivisionId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Cascade);
                _ = b.HasOne(x => x.Role).WithMany(s => s.RoleNotificationSettings).HasForeignKey(s => s.RoleId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Cascade);

                b.Property(x => x.TargetNotifies)
                .HasConversion(
                    t => string.Join(";", t.Select(e => e.ToString())),
                    t => t.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(t => Enum.Parse<TargetNotify>(t))
                )
                .Metadata
                .SetValueComparer(_targetComparer);
            });

            _ = modelBuilder.Entity<UserNotificationSetting>(b =>
            {
                _ = b.HasKey(x => x.Id);
                _ = b.HasOne(x => x.User).WithMany(s => s.UserNotificationSettings).HasForeignKey(s => s.UserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Cascade);

                b.Property(x => x.TargetNotifies)
                    .HasConversion(
                        t => string.Join(";", t.Select(e => e.ToString())),
                        t => t.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(t => Enum.Parse<TargetNotify>(t))
                    ).Metadata
                    .SetValueComparer(_targetComparer);
            });

            _ = modelBuilder.Entity<Subdivision>(b =>
            {
                _ = b.Property(r => r.Name).IsRequired();

                _ = b.HasData(new Subdivision { Id = CreateGuid(1), Name = DefaultSubdivision });
            });
        }

        public virtual Task<IEnumerable<SavedEntry>> SaveChangesAsync(IAuditWorker auditWorker, ITranslator translator, CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();

            IEnumerable<SavedEntry> entries = ChangeTracker
                .Entries()
                .Where(e => e.State is not (EntityState.Detached or EntityState.Unchanged))
                .Select(e => new SavedEntry(e.State, e.Entity as IBaseEntity))
                .ToArray();

            return base
                .SaveChangesAsync(cancellationToken)
                .ContinueWith(_ => entries);
        }

        public virtual async Task MigrateAsync(INotifyEventTypeGetter notifyEventTypeGetter, ProjectOptions projectOptions)
        {
            await base.Database.MigrateAsync();

            IDictionary<NotifyEventType, ContractProfile> contractProfiles = GetDefaultNotifyEventTypes(notifyEventTypeGetter);

            IEnumerable<ContractProfile> newContractProfile = await SeedContractProfile(contractProfiles);

            await SeedNotifySetting(newContractProfile);

            _ = await base.SaveChangesAsync();
        }

        private async Task SeedNotifySetting(IEnumerable<ContractProfile> contractProfiles)
        {
            foreach (ContractProfile contract in contractProfiles)
            {
                RoleNotificationSetting setting = new()
                {
                    ContractProfileId = contract.Id,
                    Enable = true,
                    RoleId = CreateGuid(1),
                    SubdivisionId = CreateGuid(1),
                    TargetNotifies = new TargetNotify[] { TargetNotify.Socket, TargetNotify.Email },
                };

                _ = await AddAsync(setting);
            }
        }

        private async Task<IEnumerable<ContractProfile>> SeedContractProfile(IDictionary<NotifyEventType, ContractProfile> contractProfileMap)
        {
            IEnumerable<NotifyEventType> notifyEventTypeEntitys = contractProfileMap.Select(d => d.Key);

            IEnumerable<string> notifyEventTypes = contractProfileMap.Select(d => d.Key.Type);

            IList<NotifyEventType> removedTypes = await NotifyEventTypes
                .Where(e => !notifyEventTypes.Contains(e.Type))
                .ToListAsync();

            RemoveRange(removedTypes);

            List<ContractProfile> newContractProfiles = new();

            foreach (NotifyEventType notifyEventType in notifyEventTypeEntitys)
            {
                bool exist = await NotifyEventTypes.AnyAsync(t => t.Type == notifyEventType.Type);

                if (!exist)
                {
                    _ = await AddAsync(notifyEventType);
                    _ = await AddAsync(contractProfileMap[notifyEventType]);

                    newContractProfiles.Add(contractProfileMap[notifyEventType]);
                }
                else
                {
                    //TODO: update contract profile?
                }
            }

            return newContractProfiles;
        }

        private IList<ContractSettingPropperty> GetContractProppertyTemplateHeader(string eventType, INotifyEventTypeGetter notifyEventTypeGetter)
        {
            List<ContractSettingPropperty> contractPropperties = new();

            IDictionary<string, string> jsonCustomPropperties = notifyEventTypeGetter.GetNotifyEventTypePropperties(eventType);

            for (byte i = 0; i < jsonCustomPropperties.Count; i++)
            {
                string propertyName = jsonCustomPropperties.Skip(i).First().Key;

                contractPropperties.Add(new()
                {
                    ContractName = propertyName,
                    Position = i
                });
            }

            return contractPropperties;
        }

        private IDictionary<NotifyEventType, ContractProfile> GetDefaultNotifyEventTypes(INotifyEventTypeGetter notifyEventTypeGetter)
        {
            IEnumerable<NotifyEventType> notifyEventTypes = notifyEventTypeGetter
                .GetAllNotifyEventTypes()
                .Select((t, i) => new NotifyEventType
                {
                    Id = CreateGuid(i + 1),
                    Type = t
                }
            );

            Dictionary<NotifyEventType, ContractProfile> contractProfiles = new();

            foreach (NotifyEventType notifyEventType in notifyEventTypes)
            {
                string eventType = notifyEventType.Type;

                ContractProfile contractProfile = new()
                {
                    Id = notifyEventType.Id,
                    Comment = "Стандартный профиль уведомления",
                    Name = "Стандартный профиль",
                    NotifyEventType = eventType,
                };

                ContractSettingLine contractSettingLine = new()
                {
                    Id = notifyEventType.Id,
                    Enable = true,
                    UserProppertyName = string.Empty,
                    UserTemplate = notifyEventTypeGetter.GetNotifyEventTypeDefaultUserTemplate(eventType),
                    ContractProfileId = contractProfile.Id,
                };

                IEnumerable<ContractSettingPropperty> seedingProp =
                    GetContractProppertyTemplateHeader(eventType, notifyEventTypeGetter)
                        .Select((p, i) => new ContractSettingPropperty
                        {
                            Id = Guid.NewGuid(),
                            ContractSettingLineId = contractSettingLine.Id,
                            ContractName = p.ContractName,
                            Position = p.Position
                        });

                contractSettingLine.ContractPropperties = new List<ContractSettingPropperty>(seedingProp);
                contractProfile.ContractSettingLines = new List<ContractSettingLine> { contractSettingLine };

                contractProfiles.Add(notifyEventType, contractProfile);
            }

            return contractProfiles;
        }

        public static Guid CreateGuid(int value, int index)
        {
            byte[] bytes = new byte[8];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(index, 0, 0, bytes);
        }

        public static Guid CreateGuid(int value)
        {
            byte[] bytes = new byte[16];

            BitConverter.GetBytes(value).CopyTo(bytes, 0);

            return new Guid(bytes);
        }

        public static Guid CreateGuid(string value)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
                return new Guid(hash);
            }
        }
    }
}
