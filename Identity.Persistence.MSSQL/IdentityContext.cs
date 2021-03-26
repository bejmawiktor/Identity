using Identity.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Identity.Persistence.MSSQL
{
    public class IdentityContext : DbContext
    {
        internal DbSet<ResourceDto> Resources { get; set; }
        internal DbSet<PermissionDto> Permissions { get; set; }
        internal DbSet<RoleDto> Roles { get; set; }
        internal DbSet<UserDto> Users { get; set; }

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

            modelBuilder.Entity<ResourceDto>(r =>
            {
                r.HasKey(r => r.Id);
                r.Property(r => r.Description).HasMaxLength(2000).IsRequired();
                r.ToTable("Resources");

                this.AddResourcesData(r);
            });
            modelBuilder.Entity<PermissionDto>(p =>
            {
                p.HasKey(p => new { p.Name, p.ResourceId });
                p.Property(p => p.Name).IsRequired();
                p.Property(p => p.ResourceId).IsRequired();
                p.Property(p => p.Description).HasMaxLength(2000).IsRequired();
                p.ToTable("Permissions");

                this.AddPermissionsData(p);
            });
            modelBuilder.Entity<RoleDto>(r =>
            {
                r.HasKey(r => r.Id);
                r.Property(r => r.Id).HasColumnType("UNIQUEIDENTIFIER");
                r.Property(r => r.Name);
                r.Property(r => r.Description).HasMaxLength(2000);
                r.HasMany(r => r.Permissions).WithOne(p => p.RoleDto).HasForeignKey(p => p.RoleId);
                r.ToTable("Roles");

                this.AddRolesData(r, adminRoleId);
            });
            modelBuilder.Entity<RolePermissionDto>(r =>
            {
                r.HasKey(p => new { p.PermissionName, p.PermissionResourceId, p.RoleId });
                r.Property(r => r.PermissionName);
                r.Property(r => r.PermissionResourceId);
                r.Property(r => r.RoleId).HasColumnType("UNIQUEIDENTIFIER");
                r.ToTable("RolesPermissions");

                this.AddRolesPermissionsData(r, adminRoleId);
            });
            modelBuilder.Entity<UserDto>(u =>
            {
                u.HasKey(u => u.Id);
                u.Property(u => u.Id).HasColumnType("UNIQUEIDENTIFIER");
                u.Property(u => u.Email);
                u.Property(u => u.HashedPassword).HasMaxLength(2000);
                u.HasMany(u => u.Roles).WithOne(r => r.UserDto).HasForeignKey(r => r.UserId);
                u.HasMany(u => u.Permissions).WithOne(p => p.UserDto).HasForeignKey(p => p.UserId);
                u.ToTable("Users");
            });
            modelBuilder.Entity<UserPermissionDto>(u =>
            {
                u.HasKey(p => new { p.PermissionName, p.PermissionResourceId, p.UserId });
                u.Property(u => u.PermissionName);
                u.Property(u => u.PermissionResourceId);
                u.Property(u => u.UserId).HasColumnType("UNIQUEIDENTIFIER");
                u.ToTable("UsersPermissions");
            });
            modelBuilder.Entity<UserRoleDto>(u =>
            {
                u.HasKey(p => new { p.UserId, p.RoleId });
                u.Property(u => u.UserId).HasColumnType("UNIQUEIDENTIFIER");
                u.Property(u => u.RoleId).HasColumnType("UNIQUEIDENTIFIER");
                u.ToTable("UsersRoles");
            });
        }

        private void AddResourcesData(EntityTypeBuilder<ResourceDto> entityBuilder)
        {
            entityBuilder.HasData(
                new ResourceDto(
                    id: "Identity",
                    description: "Identity service responsible for "
                        + "authentication and authorization of users."));
        }

        private void AddPermissionsData(EntityTypeBuilder<PermissionDto> entityBuilder)
        {
            entityBuilder.HasData(
                new PermissionDto()
                {
                    ResourceId = "Identity",
                    Name = "CreateUser",
                    Description = "It allows to create new users."
                },
                new PermissionDto()
                {
                    ResourceId = "Identity",
                    Name = "CreateRole",
                    Description = "It allows to create new roles."
                },
                new PermissionDto()
                {
                    ResourceId = "Identity",
                    Name = "CreateResource",
                    Description = "It allows to create new resources."
                },
                new PermissionDto()
                {
                    ResourceId = "Identity",
                    Name = "CreatePermission",
                    Description = "It allows to create new permissions."
                });
        }

        private void AddRolesData(EntityTypeBuilder<RoleDto> entityBuilder, Guid adminRoleId)
        {
            entityBuilder.HasData(
                new RoleDto()
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    Description = "Administrator role."
                });
        }

        private void AddRolesPermissionsData(EntityTypeBuilder<RolePermissionDto> entityBuilder, Guid adminRoleId)
        {
            entityBuilder.HasData(
                new RolePermissionDto()
                {
                    RoleId = adminRoleId,
                    PermissionResourceId = "Identity",
                    PermissionName = "CreateUser",
                });
            entityBuilder.HasData(
                new RolePermissionDto()
                {
                    RoleId = adminRoleId,
                    PermissionResourceId = "Identity",
                    PermissionName = "CreateRole",
                });
            entityBuilder.HasData(
                new RolePermissionDto()
                {
                    RoleId = adminRoleId,
                    PermissionResourceId = "Identity",
                    PermissionName = "CreateResource",
                });
            entityBuilder.HasData(
                new RolePermissionDto()
                {
                    RoleId = adminRoleId,
                    PermissionResourceId = "Identity",
                    PermissionName = "CreatePermission",
                });
        }
    }
}