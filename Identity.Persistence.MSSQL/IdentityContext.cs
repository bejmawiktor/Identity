using Identity.Application;
using Microsoft.EntityFrameworkCore;
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
    }
}