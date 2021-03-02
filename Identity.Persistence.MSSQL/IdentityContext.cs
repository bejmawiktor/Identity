using Identity.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Identity.Persistence.MSSQL
{
    public class IdentityContext : DbContext
    {
        internal DbSet<ResourceDto> Resources { get; set; }
        public string ConnectionString { get; }

        public IdentityContext(string connectionString)
        {
            this.ConnectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                this.ConnectionString,
                b => b.MigrationsAssembly("Identity.Persistence.MSSQL"));
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