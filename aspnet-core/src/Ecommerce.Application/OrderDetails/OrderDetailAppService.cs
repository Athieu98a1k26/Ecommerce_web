using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Ecommerce.Entitys;
using Ecommerce.Orders.Dto;
using Ecommerce.OrderDetails.Dto;
using Ecommerce.ProductStores.Dto;
using Ecommerce.Entity;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.OrderDetails
{
    public interface IOrderDetailAppService
    {
        Task<PagedResultDto<OrderDetailDto>> GetPaging(OrderDetailRequestModel request);
    }
    public class OrderDetailAppService : EcommerceAppServiceBase, IOrderDetailAppService
    {
        private readonly IRepository<OrderDetail, long> _orderDetailRepository;
        private readonly IRepository<ProductStore, long> _productStoreRepository;
        private readonly IRepository<Product, long> _productRepository;
        private readonly IRepository<Store, long> _storeRepository;
        public OrderDetailAppService(IRepository<OrderDetail, long> orderDetailRepository, IRepository<ProductStore, long> productStoreRepository, IRepository<Product, long> productRepository, IRepository<Store, long> storeRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _productStoreRepository = productStoreRepository;
            _productRepository = productRepository;
            _storeRepository = storeRepository;
        }

        [HttpPost]
        public async Task<PagedResultDto<OrderDetailDto>> GetPaging(OrderDetailRequestModel request)
        {
            var query = _orderDetailRepository.GetAll();

            var pagedAndFiltered = query
                .OrderBy(request.Sorting ?? "Id desc")
                .PageBy(request);

            var totalCount = await query.CountAsync();

            var listData = await pagedAndFiltered.ToListAsync();

            List<OrderDetailDto> listDataModel = ObjectMapper.Map<List<OrderDetailDto>>(listData);

            if (listDataModel.Count == 0)
            {
                return new PagedResultDto<OrderDetailDto>(
                   totalCount,
                   listDataModel
                );
            }

            List<long> listProductStoreId = listDataModel.Select(s=>s.ProductStoreId).ToList();

            List<ProductStoreDto> productStoreDtos = await GetListProductStore(listProductStoreId);

            foreach (OrderDetailDto detail in listDataModel)
            {
                ProductStoreDto productStoreDto = productStoreDtos.FirstOrDefault(s => s.Id == detail.ProductStoreId);

                if (productStoreDto == null)
                {
                    throw new UserFriendlyException(L("ProductStoreNotFound"));
                }

                detail.ProductName = productStoreDto.ProductName;
                detail.StoreName = productStoreDto.StoreName;
            }

            return new PagedResultDto<OrderDetailDto>(
                   totalCount,
                   listDataModel
                );
        }

        private async Task<List<ProductStoreDto>> GetListProductStore(List<long> listProductStoreId)
        {
            var query = from ps in _productStoreRepository.GetAll()
                        join p in _productRepository.GetAll() on ps.ProductCode equals p.Code into prodGroup
                        from p in prodGroup.DefaultIfEmpty()  // LEFT JOIN Product
                        join s in _storeRepository.GetAll() on ps.StoreCode equals s.Code into shopGroup
                        from s in shopGroup.DefaultIfEmpty()  // LEFT JOIN Shop
                        select new ProductStoreDto
                        {
                            Id = ps.Id,
                            StoreCode = ps.StoreCode,
                            ProductCode = ps.ProductCode,
                            ProductName = p != null ? p.Name : null,
                            StoreName = s != null ? s.Name : null
                        };
            query = query.Where(s => listProductStoreId.Contains(s.Id));

            return await query.ToListAsync();
        }
    }
}
