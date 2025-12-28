using Abp.Authorization;
using Ecommerce.Authorization.Roles;
using Ecommerce.Authorization.Users;

namespace Ecommerce.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
