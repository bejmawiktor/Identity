using Identity.Persistence.MSSQL.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Identity.Persistence.MSSQL
{
    using Application = Identity.Persistence.MSSQL.DataModels.Application;

    public class IdentityContext : DbContext
    {
        internal DbSet<Resource> Resources { get; set; }
        internal DbSet<Permission> Permissions { get; set; }
        internal DbSet<Role> Roles { get; set; }
        internal DbSet<User> Users { get; set; }
        internal DbSet<Application> Applications { get; set; }
        internal DbSet<AuthorizationCode> AuthorizationCodes { get; set; }
        internal DbSet<RefreshToken> RefreshTokens { get; set; }

        public IdentityContext(string connectionString)
        : base(GetDefaultOptions(connectionString ?? throw new ArgumentNullException(nameof(connectionString))))
        {
        }

        private static DbContextOptions GetDefaultOptions(string connectionString = null)
        {
            return new DbContextOptionsBuilder().UseSqlServer(connectionString).Options;
        }

        internal IdentityContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var adminRoleId = Guid.NewGuid();

            modelBuilder.Entity<Resource>(r =>
            {
                r.HasKey(r => r.Id);
                r.Property(r => r.Description).HasMaxLength(2000).IsRequired();
                r.ToTable("Resources");

                this.AddResourcesData(r);
            });
            modelBuilder.Entity<Permission>(p =>
            {
                p.HasKey(p => new { p.Name, p.ResourceId });
                p.Property(p => p.Name).IsRequired();
                p.Property(p => p.ResourceId).IsRequired();
                p.Property(p => p.Description).HasMaxLength(2000).IsRequired();
                p.ToTable("Permissions");

                this.AddPermissionsData(p);
            });
            modelBuilder.Entity<Role>(r =>
            {
                r.HasKey(r => r.Id);
                r.Property(r => r.Id).HasColumnType("UNIQUEIDENTIFIER");
                r.Property(r => r.Name);
                r.Property(r => r.Description).HasMaxLength(2000);
                r.HasMany(r => r.Permissions).WithOne(p => p.Role).HasForeignKey(p => p.RoleId);
                r.ToTable("Roles");

                this.AddRolesData(r, adminRoleId);
            });
            modelBuilder.Entity<Application>(a =>
            {
                a.HasKey(r => r.Id);
                a.Property(r => r.Id).HasColumnType("UNIQUEIDENTIFIER");
                a.Property(r => r.UserId).HasColumnType("UNIQUEIDENTIFIER");
                a.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId);
                a.Property(r => r.Name);
                a.Property(r => r.HomepageUrl).HasMaxLength(2000);
                a.Property(r => r.CallbackUrl).HasMaxLength(2000);
                a.ToTable("Applications");
            });
            modelBuilder.Entity<RefreshToken>(r =>
            {
                r.HasKey(r => r.Id);
                r.Property(r => r.Used).IsRequired();
                r.ToTable("RefreshTokens");
            });
            modelBuilder.Entity<RolePermission>(r =>
            {
                r.HasKey(p => new { p.PermissionName, p.PermissionResourceId, p.RoleId });
                r.Property(r => r.PermissionName);
                r.Property(r => r.PermissionResourceId);
                r.Property(r => r.RoleId).HasColumnType("UNIQUEIDENTIFIER");
                r.ToTable("RolesPermissions");

                this.AddRolesPermissionsData(r, adminRoleId);
            });
            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(u => u.Id);
                u.Property(u => u.Id).HasColumnType("UNIQUEIDENTIFIER");
                u.Property(u => u.Email);
                u.Property(u => u.HashedPassword).HasMaxLength(2000);
                u.HasMany(u => u.Roles).WithOne(r => r.User).HasForeignKey(r => r.UserId);
                u.HasMany(u => u.Permissions).WithOne(p => p.User).HasForeignKey(p => p.UserId);
                u.ToTable("Users");
            });
            modelBuilder.Entity<UserPermission>(u =>
            {
                u.HasKey(p => new { p.PermissionName, p.PermissionResourceId, p.UserId });
                u.Property(u => u.PermissionName);
                u.Property(u => u.PermissionResourceId);
                u.Property(u => u.UserId).HasColumnType("UNIQUEIDENTIFIER");
                u.ToTable("UsersPermissions");
            });
            modelBuilder.Entity<UserRole>(u =>
            {
                u.HasKey(p => new { p.UserId, p.RoleId });
                u.Property(u => u.UserId).HasColumnType("UNIQUEIDENTIFIER");
                u.Property(u => u.RoleId).HasColumnType("UNIQUEIDENTIFIER");
                u.ToTable("UsersRoles");
            });
            modelBuilder.Entity<AuthorizationCode>(a =>
            {
                a.HasKey(p => new { p.Code, p.ApplicationId });
                a.Property(p => p.Code);
                a.Property(p => p.ApplicationId).HasColumnType("UNIQUEIDENTIFIER");
                a.Property(p => p.ExpiresAt);
                a.Property(p => p.Used);
                a.HasMany(u => u.Permissions).WithOne(p => p.AuthorizationCode).HasForeignKey(p => new { p.Code, p.ApplicationId });
                a.ToTable("AuthorizationCodes");
            });
            modelBuilder.Entity<AuthorizationCodePermission>(u =>
            {
                u.HasKey(p => new { p.PermissionName, p.PermissionResourceId, p.ApplicationId, p.Code });
                u.Property(u => u.PermissionName);
                u.Property(u => u.PermissionResourceId);
                u.Property(u => u.ApplicationId).HasColumnType("UNIQUEIDENTIFIER");
                u.Property(u => u.Code);
                u.ToTable("AuthorizationCodesPermissions");
            });
        }

        private void AddResourcesData(EntityTypeBuilder<Resource> entityBuilder)
        {
            entityBuilder.HasData(
                new Resource
                {
                    Id = "Identity",
                    Description = "Identity service responsible for "
                        + "authentication and authorization of users."
                });
        }

        private void AddPermissionsData(EntityTypeBuilder<Permission> entityBuilder)
        {
            entityBuilder.HasData(
                new Permission()
                {
                    ResourceId = "Identity",
                    Name = "CreateUser",
                    Description = "It allows to create new users."
                },
                new Permission()
                {
                    ResourceId = "Identity",
                    Name = "CreateRole",
                    Description = "It allows to create new roles."
                },
                new Permission()
                {
                    ResourceId = "Identity",
                    Name = "CreateResource",
                    Description = "It allows to create new resources."
                },
                new Permission()
                {
                    ResourceId = "Identity",
                    Name = "CreatePermission",
                    Description = "It allows to create new permissions."
                });
        }

        private void AddRolesData(EntityTypeBuilder<Role> entityBuilder, Guid adminRoleId)
        {
            entityBuilder.HasData(
                new Role()
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    Description = "Administrator role."
                });
        }

        private void AddRolesPermissionsData(EntityTypeBuilder<RolePermission> entityBuilder, Guid adminRoleId)
        {
            entityBuilder.HasData(
                new RolePermission()
                {
                    RoleId = adminRoleId,
                    PermissionResourceId = "Identity",
                    PermissionName = "CreateUser",
                });
            entityBuilder.HasData(
                new RolePermission()
                {
                    RoleId = adminRoleId,
                    PermissionResourceId = "Identity",
                    PermissionName = "CreateRole",
                });
            entityBuilder.HasData(
                new RolePermission()
                {
                    RoleId = adminRoleId,
                    PermissionResourceId = "Identity",
                    PermissionName = "CreateResource",
                });
            entityBuilder.HasData(
                new RolePermission()
                {
                    RoleId = adminRoleId,
                    PermissionResourceId = "Identity",
                    PermissionName = "CreatePermission",
                });
        }
    }
}