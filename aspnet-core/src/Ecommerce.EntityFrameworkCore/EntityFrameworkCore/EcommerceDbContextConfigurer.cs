using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.EntityFrameworkCore
{
    public static class EcommerceDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<EcommerceDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<EcommerceDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
