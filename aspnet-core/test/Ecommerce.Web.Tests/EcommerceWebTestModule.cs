using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Ecommerce.EntityFrameworkCore;
using Ecommerce.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Ecommerce.Web.Tests
{
    [DependsOn(
        typeof(EcommerceWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class EcommerceWebTestModule : AbpModule
    {
        public EcommerceWebTestModule(EcommerceEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EcommerceWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(EcommerceWebMvcModule).Assembly);
        }
    }
}