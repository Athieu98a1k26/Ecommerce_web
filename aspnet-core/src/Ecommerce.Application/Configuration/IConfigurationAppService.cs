using System.Threading.Tasks;
using Ecommerce.Configuration.Dto;

namespace Ecommerce.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
