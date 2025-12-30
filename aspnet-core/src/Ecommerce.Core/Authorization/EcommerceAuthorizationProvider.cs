using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Ecommerce.Authorization
{
    public class EcommerceAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_Products, L("Products"));
            context.CreatePermission(PermissionNames.Pages_Stores, L("Stores"));
            context.CreatePermission(PermissionNames.Pages_ProductStores, L("ProductStores"));
            context.CreatePermission(PermissionNames.Pages_Orders, L("Orders"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, EcommerceConsts.LocalizationSourceName);
        }
    }
}
