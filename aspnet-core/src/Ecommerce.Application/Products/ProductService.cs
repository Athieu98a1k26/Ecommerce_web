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

namespace Ecommerce.Services
{
    public interface IProductService
    {
        Task<PagedResultDto<ProductResponseModel>> GetPaging(BaseRequest baseRequest);

        Task<ProductResponseModel> Get(long id);
    }


    [AbpAuthorize(PermissionNames.Pages_Products)]
    public class ProductService: EcommerceAppServiceBase,IProductService
    {
        private readonly IRepository<Product,long> _productRepository;
        public ProductService(IRepository<Product, long> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        public async Task<PagedResultDto<ProductResponseModel>> GetPaging(BaseRequest baseRequest)
        {
            var query = _productRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(baseRequest.Search),s => s.Code.Contains(baseRequest.Search) || s.Name.Contains(baseRequest.Search));

            var pagedAndFiltered = query
                .OrderBy(baseRequest.Sorting ?? "id desc")
                .PageBy(baseRequest);

            var totalCount = await query.CountAsync();

            var listData = await pagedAndFiltered.ToListAsync();

            var listDataModel = ObjectMapper.Map<List<ProductResponseModel>>(listData);

            return new PagedResultDto<ProductResponseModel>(
               totalCount,
               listDataModel
           );
        }

        public async Task<ProductResponseModel> Get(long id)
        {
            var data = _productRepository.Get(id);
            var dataModel = ObjectMapper.Map<ProductResponseModel>(data);

            return dataModel;
        }

        public async Task CreateOrEdit(ProductRequestModel request)
        {
            Validate(request);

            if (request.Id == null)
            {
                var data= ObjectMapper.Map<Product>(request);
                _productRepository.Insert(data);
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
                _productRepository.Update(data);
            }
        }

        private void Validate(ProductRequestModel request)
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

            if(_productRepository.GetAll().Any(s=>(s.Code == request.Code || s.Name == request.Name) && s.Id != request.Id))
            {
                throw new UserFriendlyException(L("ProductDuplicate"));
            }
        }
    }
}
