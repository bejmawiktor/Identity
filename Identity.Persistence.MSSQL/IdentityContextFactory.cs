using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Identity.Persistence.MSSQL
{
    public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            if(args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if(args.Length == 0)
            {
                throw new ArgumentException("Args must contain connectionString.");
            }

            return new IdentityContext(args[0]);
        }
    }
}