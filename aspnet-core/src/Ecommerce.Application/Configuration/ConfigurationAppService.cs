using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Ecommerce.Configuration.Dto;

namespace Ecommerce.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : EcommerceAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
