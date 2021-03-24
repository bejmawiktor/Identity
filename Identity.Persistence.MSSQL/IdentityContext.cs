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
    }
}