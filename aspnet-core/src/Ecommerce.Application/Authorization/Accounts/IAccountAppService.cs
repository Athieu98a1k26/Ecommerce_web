using System.Threading.Tasks;
using Abp.Application.Services;
using Ecommerce.Authorization.Accounts.Dto;

namespace Ecommerce.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
