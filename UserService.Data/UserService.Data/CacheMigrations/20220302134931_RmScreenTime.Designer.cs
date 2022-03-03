﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserService.Data;

namespace UserService.Data.CacheMigrations
{
    [DbContext(typeof(CacheContext))]
    [Migration("20220302134931_RmScreenTime")]
    partial class RmScreenTime
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("UserService.Core.Entity.Audit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Action")
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .HasColumnType("TEXT");

                    b.Property<string>("IpAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<string>("Position")
                        .HasColumnType("TEXT");

                    b.Property<string>("Roles")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subdivision")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Audit");
                });

            modelBuilder.Entity("UserService.Core.Entity.ContractProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("NotifyEventType")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NotifyEventType");

                    b.ToTable("ContractProfiles");
                });

            modelBuilder.Entity("UserService.Core.Entity.ContractSettingLine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ContractProfileId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enable")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LineNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserProppertyName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserTemplate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ContractProfileId");

                    b.ToTable("ContractSettingLines");
                });

            modelBuilder.Entity("UserService.Core.Entity.ContractSettingPropperty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ContractName")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ContractSettingLineId")
                        .HasColumnType("TEXT");

                    b.Property<byte>("Position")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ContractSettingLineId");

                    b.ToTable("ContractPropperties");
                });

            modelBuilder.Entity("UserService.Core.Entity.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<IDictionary<string, string>>("JsonData")
                        .HasColumnType("jsonb");

                    b.Property<string>("NotifyEventType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ObjectId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("UserService.Core.Entity.NotifyEventType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Type")
                        .IsUnique();

                    b.ToTable("NotifyEventTypes");
                });

            modelBuilder.Entity("UserService.Core.Entity.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RoleExpiration")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000001-0000-0000-0000-000000000000"),
                            RoleExpiration = new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999)
                        });
                });

            modelBuilder.Entity("UserService.Core.Entity.RoleNotificationSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ContractProfileId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enable")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SubdivisionId")
                        .HasColumnType("TEXT");

                    b.Property<string>("TargetNotifies")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ContractProfileId");

                    b.HasIndex("RoleId");

                    b.HasIndex("SubdivisionId");

                    b.ToTable("RoleNotificationSettings");
                });

            modelBuilder.Entity("UserService.Core.Entity.Subdivision", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Subdivisions");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000001-0000-0000-0000-000000000000")
                        });
                });

            modelBuilder.Entity("UserService.Core.Entity.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PasswordExpiration")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RegistredDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SubdivisionId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UserExpiration")
                        .HasColumnType("TEXT");

                    b.Property<bool>("UserLock")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("SubdivisionId");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000001-0000-0000-0000-000000000000"),
                            Email = "developer@bergen.tech",
                            PasswordExpiration = new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999),
                            RegistredDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SubdivisionId = new Guid("00000001-0000-0000-0000-000000000000"),
                            UserExpiration = new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999),
                            UserLock = false,
                            UserName = "developer"
                        });
                });

            modelBuilder.Entity("UserService.Core.Entity.UserClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "ClaimValue", "ClaimType")
                        .IsUnique();

                    b.ToTable("AspNetUserClaims");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000001-0000-0000-0000-000000000000"),
                            ClaimType = "AccessObjectIds",
                            ClaimValue = "00000001-0000-0000-0000-000000000000",
                            UserId = new Guid("00000001-0000-0000-0000-000000000000")
                        });
                });

            modelBuilder.Entity("UserService.Core.Entity.UserNotification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsRead")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("NotificationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NotificationId");

                    b.ToTable("UserNotification");
                });

            modelBuilder.Entity("UserService.Core.Entity.UserNotificationSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ContractProfileId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TargetNotifies")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ContractProfileId");

                    b.HasIndex("UserId");

                    b.ToTable("UserNotificationSettings");
                });

            modelBuilder.Entity("UserService.Core.Entity.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000001-0000-0000-0000-000000000000"),
                            RoleId = new Guid("00000001-0000-0000-0000-000000000000"),
                            UserId = new Guid("00000001-0000-0000-0000-000000000000")
                        });
                });

            modelBuilder.Entity("UserService.Core.Entity.WebHook", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ContractProfileId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.Property<int>("WebHookContractType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ContractProfileId");

                    b.ToTable("WebHooks");
                });

            modelBuilder.Entity("UserService.Core.Entity.ContractProfile", b =>
                {
                    b.HasOne("UserService.Core.Entity.NotifyEventType", null)
                        .WithMany("ContractProfiles")
                        .HasForeignKey("NotifyEventType")
                        .HasPrincipalKey("Type")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UserService.Core.Entity.ContractSettingLine", b =>
                {
                    b.HasOne("UserService.Core.Entity.ContractProfile", "ContractProfile")
                        .WithMany("ContractSettingLines")
                        .HasForeignKey("ContractProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContractProfile");
                });

            modelBuilder.Entity("UserService.Core.Entity.ContractSettingPropperty", b =>
                {
                    b.HasOne("UserService.Core.Entity.ContractSettingLine", "ContractSettingLine")
                        .WithMany("ContractPropperties")
                        .HasForeignKey("ContractSettingLineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContractSettingLine");
                });

            modelBuilder.Entity("UserService.Core.Entity.RoleNotificationSetting", b =>
                {
                    b.HasOne("UserService.Core.Entity.ContractProfile", "ContractProfile")
                        .WithMany("RoleNotificationSettings")
                        .HasForeignKey("ContractProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UserService.Core.Entity.Role", "Role")
                        .WithMany("RoleNotificationSettings")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UserService.Core.Entity.Subdivision", "Subdivision")
                        .WithMany("RoleNotificationSettings")
                        .HasForeignKey("SubdivisionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContractProfile");

                    b.Navigation("Role");

                    b.Navigation("Subdivision");
                });

            modelBuilder.Entity("UserService.Core.Entity.User", b =>
                {
                    b.HasOne("UserService.Core.Entity.Subdivision", "Subdivision")
                        .WithMany("Users")
                        .HasForeignKey("SubdivisionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subdivision");
                });

            modelBuilder.Entity("UserService.Core.Entity.UserClaim", b =>
                {
                    b.HasOne("UserService.Core.Entity.User", "User")
                        .WithMany("UserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserService.Core.Entity.UserNotification", b =>
                {
                    b.HasOne("UserService.Core.Entity.Notification", null)
                        .WithMany("UserNotifications")
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserService.Core.Entity.UserNotificationSetting", b =>
                {
                    b.HasOne("UserService.Core.Entity.ContractProfile", "ContractProfile")
                        .WithMany("UserNotificationSettings")
                        .HasForeignKey("ContractProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UserService.Core.Entity.User", "User")
                        .WithMany("UserNotificationSettings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContractProfile");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserService.Core.Entity.UserRole", b =>
                {
                    b.HasOne("UserService.Core.Entity.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UserService.Core.Entity.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserService.Core.Entity.WebHook", b =>
                {
                    b.HasOne("UserService.Core.Entity.ContractProfile", "ContractProfile")
                        .WithMany("WebHooks")
                        .HasForeignKey("ContractProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContractProfile");
                });

            modelBuilder.Entity("UserService.Core.Entity.ContractProfile", b =>
                {
                    b.Navigation("ContractSettingLines");

                    b.Navigation("RoleNotificationSettings");

                    b.Navigation("UserNotificationSettings");

                    b.Navigation("WebHooks");
                });

            modelBuilder.Entity("UserService.Core.Entity.ContractSettingLine", b =>
                {
                    b.Navigation("ContractPropperties");
                });

            modelBuilder.Entity("UserService.Core.Entity.Notification", b =>
                {
                    b.Navigation("UserNotifications");
                });

            modelBuilder.Entity("UserService.Core.Entity.NotifyEventType", b =>
                {
                    b.Navigation("ContractProfiles");
                });

            modelBuilder.Entity("UserService.Core.Entity.Role", b =>
                {
                    b.Navigation("RoleNotificationSettings");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("UserService.Core.Entity.Subdivision", b =>
                {
                    b.Navigation("RoleNotificationSettings");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("UserService.Core.Entity.User", b =>
                {
                    b.Navigation("UserClaims");

                    b.Navigation("UserNotificationSettings");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
