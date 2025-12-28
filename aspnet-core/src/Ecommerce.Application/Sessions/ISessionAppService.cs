using System.Threading.Tasks;
using Abp.Application.Services;
using Ecommerce.Sessions.Dto;

namespace Ecommerce.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
