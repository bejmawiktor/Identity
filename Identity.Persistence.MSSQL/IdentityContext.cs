using Identity.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Identity.Persistence.MSSQL
{
    public class IdentityContext : DbContext
    {
        internal DbSet<ResourceDto> Resources { get; set; }

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
                r.Property(r => r.Description).HasMaxLength(2000);
                r.ToTable("Resources");

                this.AddResourcesData(r);
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
    }
}