using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using DatabaseExtension;
using DatabaseExtension.Translator;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Newtonsoft.Json;

using UserService.Core;
using UserService.Core.AuditPackage;
using UserService.Core.DataInterfaces;
using UserService.Core.Entity;
using UserService.Core.Models;
using UserService.Core.NotifyEventTypeGetter;
using UserService.Core.PolindromHasher;
using UserService.Core.ServiceSettings;

using YamlDotNet.Serialization;

namespace UserService.Data
{
    public class DatabaseContext : UserServiceContext
    {
        private const string YamlType = ".yaml";

        private static readonly Type[] s_auditTypes = typeof(DatabaseContext)
            .GetProperties()
            .Where(p => p.PropertyType.IsGenericType && p.PropertyType == typeof(DbSet<>).MakeGenericType(p.PropertyType.GenericTypeArguments))
            .Select(p => p.PropertyType.GenericTypeArguments.Single())
            .Where(t => t.GetCustomAttribute<AuditEntityAttribute>() is not null)
            .ToArray();

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<ServiceSetting> ServiceSettings { get; set; }
        public DbSet<ResourceTag> ResourceTags { get; set; }
        public DbSet<ResourceTagRelation> ResourceTagRelations { get; set; }

        public DatabaseContext(IPasswordHasher passwordHasher, DbContextOptions<DatabaseContext> options)
            : base(passwordHasher, options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = modelBuilder.Entity<RoleClaim>(b =>
            {
                _ = b.Property(et => et.Id).ValueGeneratedNever();
                _ = b.Ignore(c => c.PermissionAssert);
                _ = b.HasKey(r => r.Id);
                _ = b.HasOne(r => r.Role).WithMany(r => r.RoleClaims).HasForeignKey(r => r.RoleId).HasPrincipalKey(r => r.Id).OnDelete(DeleteBehavior.Cascade);
                _ = b.HasOne(r => r.ResourceTag).WithMany(r => r.RoleClaims).HasForeignKey(r => r.ClaimType).HasPrincipalKey(r => r.Tag).OnDelete(DeleteBehavior.Cascade);
                _ = b.HasIndex(t => new { t.RoleId, t.ClaimType }).IsUnique();
            });

            _ = modelBuilder.Entity<Permission>(f =>
            {
                _ = f.Property(r => r.Name).IsRequired();

                _ = f.HasMany(p => p.ResourceTags).WithMany(t => t.Permissions);
                _ = f.HasMany(p => p.LockedResourceTags).WithMany(t => t.LockedPermissions);
                _ = f.HasMany(p => p.MotherPermissions).WithMany(p => p.ChildPermissions);

                _ = f.HasData(new Permission { Id = CreateGuid(1), Name = "Default" });
            });

            _ = modelBuilder.Entity($"{nameof(Permission)}{nameof(Role)}").HasData(new
            {
                PermissionsId = CreateGuid(1),
                RolesId = CreateGuid(1),
            });

            _ = modelBuilder.Entity<ServiceSetting>(b =>
            {
                _ = b.HasKey(r => r.Id);
                _ = b.HasIndex(r => r.ServiceSettingAttribute).IsUnique();
            });

            _ = modelBuilder.Entity<ServiceSetting>().HasData(
                Enum.GetValues<ServiceSettingAttribute>()
                    .Select((a, i) => new ServiceSetting
                    {
                        Id = CreateGuid(i + 1),
                        ServiceSettingAttribute = a,
                        ServiceSettingValue = string.Empty,
                    }
                ));

            _ = modelBuilder.Entity<ResourceTag>(f =>
            {
                _ = f.HasMany(p => p.ResourceTagRelations).WithOne(r => r.DependentResourceTag).HasForeignKey(r => r.DependentResourceTagValue).HasPrincipalKey(r => r.Tag).OnDelete(DeleteBehavior.Cascade);
                _ = f.HasMany(p => p.DependentResourceTagRelations).WithOne(r => r.ResourceTag).HasForeignKey(r => r.ResourceTagValue).HasPrincipalKey(r => r.Tag).OnDelete(DeleteBehavior.Cascade);
            });

            _ = modelBuilder.Entity<Position>(f =>
            {
                _ = f.Property(r => r.Name).IsRequired();
            });
        }

        public override async Task<IEnumerable<SavedEntry>> SaveChangesAsync(IAuditWorker auditWorker, ITranslator translator, CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();

            IEnumerable<SavedEntry> entries = ChangeTracker
                .Entries()
                .Where(e => e.State is not (EntityState.Detached or EntityState.Unchanged))
                .Where(e => e.Entity is IBaseEntity)
                .Select(e => new SavedEntry(e.State, e.Entity as IBaseEntity))
                .ToArray();

            _ = await base.SaveChangesAsync(cancellationToken);

            IEnumerable<SavedEntry> auditEntries = entries
                .Where(e => s_auditTypes.Contains(e.Entity.GetType()));

            if (!auditEntries.Any())
            {
                return entries;
            }

            foreach (SavedEntry entry in auditEntries)
            {
                IBaseEntity baseEntity = entry.Entity;

                string action = translator.GetUserText<EntityState>(entry.State.ToString());
                string entity = translator.GetUserText<AuditEntityAttribute>(baseEntity.GetType().Name);

                AuditCreateCommand auditCreateCommand = new(string.Empty, $"{action} {entity} {baseEntity.AuditName}", entry.State.ToString());

                await auditWorker.CreateAudit(auditCreateCommand);
            }

            _ = await base.SaveChangesAsync(cancellationToken);

            return entries;
        }

        public override async Task MigrateAsync(INotifyEventTypeGetter notifyEventTypeGetter, ProjectOptions projectOptions)
        {
            await base.MigrateAsync(notifyEventTypeGetter, projectOptions);

            IEnumerable<ResourceTag> permissionTags = GetPermissionTag(projectOptions);

            await SeedResourceTags(permissionTags);

            await SeedEnviromentValues(projectOptions);

            _ = await base.SaveChangesAsync();
        }

        private Task SeedEnviromentValues(ProjectOptions projectOptions)
        {
            return ServiceSettings.ForEachAsync(s =>
            {
                if (!string.IsNullOrWhiteSpace(s.ServiceSettingValue))
                {
                    Environment.SetEnvironmentVariable(s.ServiceSettingAttribute.GetEnviromentName(), s.ServiceSettingValue);
                }
                else
                {
                    s.ServiceSettingValue = projectOptions.GetEnvironmentVariable(s.ServiceSettingAttribute.GetEnviromentName(), validationIgnore: true);
                    s.ValidateValue();
                }
            });
        }

        private async Task SeedResourceTags(IEnumerable<ResourceTag> resourceTags)
        {
            IEnumerable<string> tags = resourceTags.Select(p => p.Tag);

            Permission permission = await Permissions
                .Include(p => p.ResourceTags.Where(t => tags.Contains(t.Tag)))
                .SingleAsync(p => p.Id == CreateGuid(1));

            List<ResourceTag> removedTag = await ResourceTags
                .Where(p => !tags.Contains(p.Tag))
                .ToListAsync();

            ResourceTags.RemoveRange(removedTag);

            foreach (ResourceTag resourceTag in resourceTags)
            {
                bool alreadyExist = await ResourceTags.AnyAsync(p => p.Tag == resourceTag.Tag);

                if (!alreadyExist)
                {
                    RoleClaim roleClaim = new()
                    {
                        Id = resourceTag.Id,
                        ClaimType = resourceTag.Tag,
                        PermissionAssert = PermissionAssert.Remove,
                        RoleId = CreateGuid(1),
                    };

                    _ = await RoleClaims.AddAsync(roleClaim);
                    _ = await ResourceTags.AddAsync(resourceTag);

                    permission.ResourceTags.Add(resourceTag);
                }
                else
                {
                    ResourceTagRelation[] resourceTagRelations = await ResourceTags
                        .Where(t => t.Tag == resourceTag.Tag)
                        .SelectMany(t => t.ResourceTagRelations)
                        .ToArrayAsync();

                    IEnumerable<ResourceTagRelation> newRelations = resourceTag.ResourceTagRelations
                        .Where(r => !resourceTagRelations.Any(e => e.ResourceTagValue == r.ResourceTagValue));

                    IEnumerable<ResourceTagRelation> rmRelations = resourceTagRelations
                        .Where(r => !resourceTag.ResourceTagRelations.Any(e => e.ResourceTagValue == r.ResourceTagValue));

                    await ResourceTagRelations.AddRangeAsync(newRelations);
                    ResourceTagRelations.RemoveRange(rmRelations);
                }
            }

            _ = Update(permission);
        }

        private IEnumerable<ResourceTag> GetPermissionTag(ProjectOptions projectOptions)
        {
            string fileRoute = projectOptions.PermissionRoute;

            using StreamReader reader = new(fileRoute);

            string json = reader.ReadToEnd();

            if (fileRoute.Contains(YamlType))
            {
                StringReader stringReader = new(json);

                IDeserializer deserializer = new DeserializerBuilder().Build();
                object yamlObject = deserializer.Deserialize(stringReader);

                ISerializer serializer = new SerializerBuilder()
                    .JsonCompatible()
                    .Build();

                json = serializer.Serialize(yamlObject);
            }

            ResourcesYamlModel permissionYamlModel = JsonConvert.DeserializeObject<ResourcesYamlModel>(json);

            return permissionYamlModel.Resources.Select((p, i) => new ResourceTag
            {
                Id = CreateGuid(p.Key),
                Name = p.Value.Name,
                Tag = p.Key,
                ResourceTagRelations = p.Value.Relations.Select((r, j) => new ResourceTagRelation
                {
                    Id = CreateGuid($"{r.Key}{r.Value}{p.Key}"),
                    ResourceTagValue = r.Key,
                    PermissionAssert = r.Value,
                    DependentResourceTagValue = p.Key,
                })
                .ToArray(),
            });
        }

        private record ResourcesYamlModel(IDictionary<string, PermissionYamlModel> Resources);
        private record PermissionYamlModel(string Name)
        {
            public IDictionary<string, PermissionAssert> Relations { get; init; } = new Dictionary<string, PermissionAssert>();
        };
    }
}
