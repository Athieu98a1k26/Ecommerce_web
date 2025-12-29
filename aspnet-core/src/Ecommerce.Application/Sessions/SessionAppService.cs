using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Abp.Auditing;
using Ecommerce.Sessions.Dto;

namespace Ecommerce.Sessions
{
    public class SessionAppService : EcommerceAppServiceBase, ISessionAppService
    {
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>()
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
            }

            var source = LocalizationManager.GetSource(EcommerceConsts.LocalizationSourceName);

            var vi = source.GetString("Users", new CultureInfo("vi"));
            var en = source.GetString("Users", new CultureInfo("en"));

            Console.WriteLine($"VI: {vi}");
            Console.WriteLine($"EN: {en}");

            return output;
        }
    }
}
