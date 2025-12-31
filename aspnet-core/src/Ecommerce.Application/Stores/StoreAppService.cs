using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Ecommerce.Authorization;
using Ecommerce.Bases;
using Ecommerce.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Ecommerce.Stores.Dto;
using Ecommerce.Entitys;


namespace Ecommerce.Stores
{
    public interface IStoreAppService
    {
        Task<PagedResultDto<StoreDto>> GetPaging(BaseRequest baseRequest);

        Task<StoreDto> Get(long id);
        Task CreateOrEdit(CreateUpdateStoreDto request);
        Task Delete(long id);
    }
    [AbpAuthorize(PermissionNames.Pages_Stores)]
    public class StoreAppService : EcommerceAppServiceBase, IStoreAppService
    {
        private readonly IRepository<Store, long> _storeRepository;
        private readonly IRepository<ProductStore, long> _productStoreRepository;
        public StoreAppService(IRepository<Store, long> storeRepository, IRepository<ProductStore, long> productStoreRepository)
        {
            _storeRepository = storeRepository;
            _productStoreRepository = productStoreRepository;
        }

        [HttpPost]
        public async Task<PagedResultDto<StoreDto>> GetPaging(BaseRequest baseRequest)
        {
            var query = _storeRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(baseRequest.Search), s => s.Code.Contains(baseRequest.Search) || s.Name.Contains(baseRequest.Search));

            var pagedAndFiltered = query
                .OrderBy(baseRequest.Sorting ?? "id desc")
                .PageBy(baseRequest);

            var totalCount = await query.CountAsync();

            var listData = await pagedAndFiltered.ToListAsync();

            var listDataModel = ObjectMapper.Map<List<StoreDto>>(listData);

            return new PagedResultDto<StoreDto>(
               totalCount,
               listDataModel
           );
        }

        public async Task<StoreDto> Get(long id)
        {
            var data = await _storeRepository.GetAsync(id);
            var dataModel = ObjectMapper.Map<StoreDto>(data);

            return dataModel;
        }

        public async Task CreateOrEdit(CreateUpdateStoreDto request)
        {
            await Validate(request);

            if (request.Id == null)
            {
                var data = ObjectMapper.Map<Store>(request);
                await _storeRepository.InsertAsync(data);
            }
            else
            {
                var data = _storeRepository.Get(request.Id ?? 0);

                if (data == null)
                {
                    throw new UserFriendlyException(L("ProductNotFound"));
                }

                data.Code = request.Code;
                data.Name = request.Name;
                data.Address = request.Address;
                await _storeRepository.UpdateAsync(data);
            }
        }

        private async Task Validate(CreateUpdateStoreDto request)
        {
            if (string.IsNullOrEmpty(request.Code))
            {
                throw new UserFriendlyException(L("CodeStoreRequired"));
            }

            if (request.Code.Length > 100)
            {
                throw new UserFriendlyException(L("CodeStoreMaxLength"));
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                throw new UserFriendlyException(L("NameStoreRequired"));
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                throw new UserFriendlyException(L("NameStoreMaxLength"));
            }

            if (await _storeRepository.GetAll().AnyAsync(s => (s.Code == request.Code || s.Name == request.Name) && s.Id != request.Id))
            {
                throw new UserFriendlyException(L("StoreDuplicate"));
            }
        }

        public async Task Delete(long id)
        {
            await ValidateDelete(id);
            await _productStoreRepository.DeleteAsync(id);
        }

        public async Task ValidateDelete(long id)
        {
            var product = _storeRepository.Get(id);

            if (await _productStoreRepository.GetAll().AnyAsync(s => s.ProductCode == product.Code))
            {
                throw new UserFriendlyException(L("StoreUse"));
            }
        }
    }
}
