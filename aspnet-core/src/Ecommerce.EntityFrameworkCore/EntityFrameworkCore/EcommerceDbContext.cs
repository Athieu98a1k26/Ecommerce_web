using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Ecommerce.Authorization.Roles;
using Ecommerce.Authorization.Users;
using Ecommerce.MultiTenancy;

namespace Ecommerce.EntityFrameworkCore
{
    public class EcommerceDbContext : AbpZeroDbContext<Tenant, Role, User, EcommerceDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
            : base(options)
        {
        }
    }
}
