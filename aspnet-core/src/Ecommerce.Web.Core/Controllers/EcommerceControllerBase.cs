using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Controllers
{
    public abstract class EcommerceControllerBase: AbpController
    {
        protected EcommerceControllerBase()
        {
            LocalizationSourceName = EcommerceConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
