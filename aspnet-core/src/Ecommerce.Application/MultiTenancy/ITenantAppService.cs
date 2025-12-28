using Abp.Application.Services;
using Ecommerce.MultiTenancy.Dto;

namespace Ecommerce.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

