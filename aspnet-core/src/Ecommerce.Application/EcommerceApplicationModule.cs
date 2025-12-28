using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Ecommerce.Authorization;

namespace Ecommerce
{
    [DependsOn(
        typeof(EcommerceCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class EcommerceApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<EcommerceAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(EcommerceApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
