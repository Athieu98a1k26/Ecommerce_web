using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Ecommerce.Authorization;
using Ecommerce.Bases;
using Ecommerce.Entity;
using Ecommerce.Products.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Abp.UI;
using Ecommerce.Entitys;

namespace Ecommerce.Services
{
    public interface IProductAppService
    {
        Task<PagedResultDto<ProductDto>> GetPaging(BaseRequest baseRequest);

        Task<ProductDto> Get(long id);
        Task CreateOrEdit(CreateUpdateProductDto request);
        Task Delete(long id);
    }


    [AbpAuthorize(PermissionNames.Pages_Products)]
    public class ProductAppService: EcommerceAppServiceBase, IProductAppService
    {
        private readonly IRepository<Product,long> _productRepository;
        private readonly IRepository<ProductStore, long> _productStoreRepository;
        public ProductAppService(IRepository<Product, long> productRepository, IRepository<ProductStore, long> productStoreRepository)
        {
            _productRepository = productRepository;
            _productStoreRepository = productStoreRepository;
        }


        [HttpPost]
        public async Task<PagedResultDto<ProductDto>> GetPaging(BaseRequest baseRequest)
        {
            var query = _productRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(baseRequest.Search),s => s.Code.Contains(baseRequest.Search) || s.Name.Contains(baseRequest.Search));

            var pagedAndFiltered = query
                .OrderBy(baseRequest.Sorting ?? "Id desc")
                .PageBy(baseRequest);

            var totalCount = await query.CountAsync();

            var listData = await pagedAndFiltered.ToListAsync();

            var listDataModel = ObjectMapper.Map<List<ProductDto>>(listData);

            return new PagedResultDto<ProductDto>(
               totalCount,
               listDataModel
            );

        }

        public async Task<ProductDto> Get(long id)
        {
            var data =await _productRepository.GetAsync(id);
            var dataModel = ObjectMapper.Map<ProductDto>(data);

            return dataModel;
        }

        public async Task CreateOrEdit(CreateUpdateProductDto request)
        {
            await Validate(request);

            if (request.Id == null)
            {
                var data= ObjectMapper.Map<Product>(request);
                await _productRepository.InsertAsync(data);
            }
            else
            {
                var data = _productRepository.Get(request.Id??0);

                if(data == null)
                {
                    throw new UserFriendlyException(L("ProductNotFound"));
                }    

                data.Code = request.Code;
                data.Name = request.Name;
                await _productRepository.UpdateAsync(data);
            }
        }

        private async Task Validate(CreateUpdateProductDto request)
        {
            if(string.IsNullOrEmpty(request.Code))
            {
                throw new UserFriendlyException(L("CodeProductRequired"));
            }

            if (request.Code.Length > 100)
            {
                throw new UserFriendlyException(L("CodeProductMaxLength"));
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                throw new UserFriendlyException(L("NameProductRequired"));
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                throw new UserFriendlyException(L("NameProductMaxLength"));
            }

            if(await _productRepository.GetAll().AnyAsync(s=>(s.Code == request.Code || s.Name == request.Name) && s.Id != request.Id))
            {
                throw new UserFriendlyException(L("ProductDuplicate"));
            }
        }

        public async Task Delete(long id)
        {
            await ValidateDelete(id);
            await _productRepository.DeleteAsync(id);
        }

        public async Task ValidateDelete(long id)
        {
            var product = _productRepository.Get(id);

            if (await _productStoreRepository.GetAll().AnyAsync(s=>s.ProductCode == product.Code))
            {
                throw new UserFriendlyException(L("ProductUse"));
            }    
        }
    }
}
