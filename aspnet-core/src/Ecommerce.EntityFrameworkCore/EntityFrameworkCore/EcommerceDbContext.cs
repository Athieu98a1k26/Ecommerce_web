using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Ecommerce.Authorization.Roles;
using Ecommerce.Authorization.Users;
using Ecommerce.MultiTenancy;
using Ecommerce.Entitys;
using Ecommerce.Entity;

namespace Ecommerce.EntityFrameworkCore
{
    public class EcommerceDbContext : AbpZeroDbContext<Tenant, Role, User, EcommerceDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Product> Products { get; set; } 
        public DbSet<Store> Stores { get; set; } 
        public DbSet<Catalog> Catalogs { get; set; } 
        public DbSet<History> Historys { get; set; } 
        public DbSet<Order> Orders { get; set; } 
        public DbSet<OrderDetail> OrderDetails { get; set; } 
        public DbSet<Person> Persons { get; set; } 
        public DbSet<ProductStore> ProductStores { get; set; } 
        public DbSet<ProductStoreDetail> ProductStoreDetails { get; set; } 


        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
            : base(options)
        {
        }
    }
}
