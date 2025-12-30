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
using Ecommerce.ProductStores.Dto;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecommerce.ProductStores
{
    public interface IProductStorePublicAppService
    {
        Task<PagedResultDto<ProductStoreDto>> GetPagingFeaturedProduct(BaseRequest baseRequest);
    }
    public class ProductStorePublicAppService : EcommerceAppServiceBase, IProductStorePublicAppService
    {
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<ProductStoreDetail, long> _productStoreDetailRepository;
        private readonly IRepository<Product, long> _productRepository;
        private readonly IRepository<ProductStore, long> _productStoreRepository;
        private readonly IRepository<Store, long> _storeRepository;
        public ProductStorePublicAppService(IRepository<ProductStoreDetail, long> productStoreDetailRepository, IRepository<ProductStore, long> productStoreRepository, IRepository<Product, long> productRepository, IRepository<Store, long> storeRepository, IRepository<Order, long> orderRepository)
        {
            _productStoreDetailRepository = productStoreDetailRepository;
            _productStoreRepository = productStoreRepository;
            _productRepository = productRepository;
            _storeRepository = storeRepository;
            _orderRepository = orderRepository;
        }

        [HttpPost]
        public async Task<PagedResultDto<ProductStoreDto>> GetPagingFeaturedProduct(BaseRequest baseRequest)
        {
            var query = from ps in _productStoreRepository.GetAll()
                        join p in _productRepository.GetAll() on ps.ProductCode equals p.Code into prodGroup
                        from p in prodGroup.DefaultIfEmpty()  // LEFT JOIN Product
                        join s in _storeRepository.GetAll() on ps.StoreCode equals s.Code into shopGroup
                        from s in shopGroup.DefaultIfEmpty()  // LEFT JOIN Shop
                        select new ProductStoreDto
                        {
                            StoreCode = ps.StoreCode,
                            ProductCode = ps.ProductCode,
                            ProductName = p != null ? p.Name : null,
                            StoreName = s != null ? s.Name : null,
                            Price = ps.Price,
                            Stars = ps.Stars,
                            Sold = ps.Sold,
                        };

            var pagedAndFiltered = query
                .OrderBy(baseRequest.Sorting ?? "Id desc")
                .PageBy(baseRequest);

            var totalCount = await query.CountAsync();

            var listData = await pagedAndFiltered.ToListAsync();

            return new PagedResultDto<ProductStoreDto>(
                   totalCount,
                   listData
                );
        }
    }
}
