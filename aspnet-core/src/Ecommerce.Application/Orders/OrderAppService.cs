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
using Ecommerce.Orders.Dto;
using Abp.Domain.Uow;
using Ecommerce.Helper;
using Ecommerce.Common;

namespace Ecommerce.Orders
{
    public interface IOrderAppService
    {
        Task<PagedResultDto<OrderDto>> GetPaging(BaseRequest baseRequest);
        Task CreateOrEdit(CreateUpdateOrderDto request);
    }
    public class OrderAppService : EcommerceAppServiceBase, IOrderAppService
    {
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<ProductStoreDetail, long> _productStoreDetailRepository;
        private readonly IRepository<Province, long> _provinceRepository;
        private readonly IRepository<ProductStore, long> _productStoreRepository;
        public OrderAppService(IRepository<Order, long> orderRepository, IRepository<ProductStoreDetail, long> productStoreDetailRepository, IRepository<ProductStore, long> productStoreRepository, IRepository<Province, long> provinceRepository)
        {
            _orderRepository = orderRepository;
            _productStoreDetailRepository = productStoreDetailRepository;
            _productStoreRepository = productStoreRepository;
            _provinceRepository = provinceRepository;
        }

        [HttpPost]
        public async Task<PagedResultDto<OrderDto>> GetPaging(BaseRequest baseRequest)
        {
            var query = _orderRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(baseRequest.Search), s => s.Code.Contains(baseRequest.Search) || s.FullName.Contains(baseRequest.Search));

            var pagedAndFiltered = query
                .OrderBy(baseRequest.Sorting ?? "Id desc")
                .PageBy(baseRequest);

            var totalCount = await query.CountAsync();

            var listData = await pagedAndFiltered.ToListAsync();

            List<OrderDto> listDataModel = ObjectMapper.Map<List<OrderDto>>(listData);

            if(listDataModel.Count == 0)
            {
                return new PagedResultDto<OrderDto>(
                   totalCount,
                   listDataModel
                );
            }

            //lấy thông tin tỉnh , xã
            List<string> listProvinceCode = new List<string>();
            
            foreach(var item in listDataModel)
            {
                listProvinceCode.Add(item.ProvinCode);
                listProvinceCode.Add(item.WardCode);
            }    

            List<Province> listProvince = await _provinceRepository.GetAll().Where(s=> listProvinceCode.Contains(s.Code)).ToListAsync();

            foreach (var item in listDataModel)
            {
                Province province = listProvince.FirstOrDefault(s=>s.Code == item.ProvinCode);

                item.ProvinName = province.Name;

                Province ward = listProvince.FirstOrDefault(s=>s.Code == item.WardCode);

                item.WardName = ward.Name;
            }

            return new PagedResultDto<OrderDto>(
               totalCount,
               listDataModel
           );
        }

        [UnitOfWork]
        public async Task CreateOrEdit(CreateUpdateOrderDto request)
        {
            await Validate(request);
            ProductStoreDetail productStoreDetail = await _productStoreDetailRepository.GetAll().FirstOrDefaultAsync(s => s.Id == request.ProductStoreDetailId);
            //thêm đơn hàng
            Order order =  ObjectMapper.Map<Order>(request);
            order.ProductStoreId = productStoreDetail.ProductStoreId;
            order.Code = OrderHelper.GenerateOrderCode();
            order.OrderStatus = CatalogType.OrderDetailStatus.First();
            await _orderRepository.InsertAsync(order);
        }



        private async Task Validate(CreateUpdateOrderDto request)
        {
            if (!request.ProductStoreDetailId.HasValue)
            {
                throw new UserFriendlyException(L("ProductStoreDetailNotFound"));
            }

            if (!await _productStoreDetailRepository.GetAll().AnyAsync(s => s.Id == request.ProductStoreDetailId))
            {
                throw new UserFriendlyException(L("ProductStoreDetailNotFound"));
            }
        }
    }
}
