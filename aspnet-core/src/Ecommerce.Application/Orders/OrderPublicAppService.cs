using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Abp.UI;
using Ecommerce.Entitys;
using Ecommerce.Orders.Dto;
using Abp.Domain.Uow;
using Ecommerce.Helper;
using Ecommerce.Common;
using Ecommerce.Persons.Dto;

namespace Ecommerce.Orders
{
    public interface IOrderPublicAppService
    {
        Task CreateOrEdit(CreateUpdateOrderDto request);
        Task<PagedResultDto<OrderDto>> GetPagingForUser(OrderRequestDto request);
    }
    public class OrderPublicAppService : EcommerceAppServiceBase, IOrderPublicAppService
    {
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<ProductStoreDetail, long> _productStoreDetailRepository;
        private readonly IRepository<Province, long> _provinceRepository;
        private readonly IRepository<OrderDetail, long> _orderDetailRepository;
        public OrderPublicAppService(IRepository<Order, long> orderRepository, IRepository<ProductStoreDetail, long> productStoreDetailRepository, IRepository<OrderDetail, long> orderDetailRepository, IRepository<Province, long> provinceRepository)
        {
            _orderRepository = orderRepository;
            _productStoreDetailRepository = productStoreDetailRepository;
            _orderDetailRepository = orderDetailRepository;
            _provinceRepository = provinceRepository;
        }

        [UnitOfWork]
        public async Task CreateOrEdit(CreateUpdateOrderDto request)
        {
            if (request.ListOrderDetailDto == null || request.ListOrderDetailDto.Count == 0)
            {
                throw new UserFriendlyException(L("ProductStoreDetailNotFound"));
            }

            List<long> listProductStoreDetailId = request.ListOrderDetailDto.Select(s => s.ProductStoreDetailId).ToList();

            await Validate(listProductStoreDetailId);

            //thêm đơn hàng
            Order order = ObjectMapper.Map<Order>(request);
            order.Code = OrderHelper.GenerateOrderCode();
            order.Count = request.ListOrderDetailDto.Sum(s => s.Count);
            order.Price = request.ListOrderDetailDto.Sum(s => s.Price);
            order.OrderStatus = OrderStatus.Init;
            long id = await _orderRepository.InsertAndGetIdAsync(order);

            //thêm vào bảng detail
            List<ProductStoreDetail> listProductStoreDetail = await _productStoreDetailRepository.GetAll().Where(s => listProductStoreDetailId.Contains(s.Id)).ToListAsync();
            foreach (var item in request.ListOrderDetailDto)
            {
                ProductStoreDetail productStoreDetail1 = listProductStoreDetail.FirstOrDefault(s => s.Id == item.ProductStoreDetailId);

                OrderDetail orderDetail = ObjectMapper.Map<OrderDetail>(item);
                orderDetail.OrderId = id;
                orderDetail.ProductStoreId = productStoreDetail1?.ProductStoreId ?? 0;
                orderDetail.Price = productStoreDetail1.Price;
                orderDetail.DetailPrice = productStoreDetail1.DetailPrice;


                await _orderDetailRepository.InsertAsync(orderDetail);
            }
        }

        private async Task Validate(List<long> listProductStoreDetailId)
        {

            if (!await _productStoreDetailRepository.GetAll().AnyAsync(s => listProductStoreDetailId.Contains(s.Id)))
            {
                throw new UserFriendlyException(L("ProductStoreDetailNotFound"));
            }
        }

        [HttpPost]
        public async Task<PagedResultDto<OrderDto>> GetPagingForUser(OrderRequestDto request)
        {
            var query = _orderRepository.GetAll().Where(s => s.PhoneNumber==request.PhoneNumber || s.Email == request.Email && (string.IsNullOrEmpty(request.OrderStatus) || s.OrderStatus == request.OrderStatus));

            var pagedAndFiltered = query
                .OrderBy(request.Sorting ?? "Id desc")
                .PageBy(request);

            var totalCount = await query.CountAsync();

            var listData = await pagedAndFiltered.ToListAsync();

            List<OrderDto> listDataModel = ObjectMapper.Map<List<OrderDto>>(listData);

            if (listDataModel.Count == 0)
            {
                return new PagedResultDto<OrderDto>(
                   totalCount,
                   listDataModel
                );
            }

            //lấy thông tin tỉnh , xã
            List<string> listProvinceCode = new List<string>();

            foreach (var item in listDataModel)
            {
                listProvinceCode.Add(item.ProvinceCode);
                listProvinceCode.Add(item.WardCode);
            }

            List<Province> listProvince = await _provinceRepository.GetAll().Where(s => listProvinceCode.Contains(s.Code)).ToListAsync();

            foreach (var item in listDataModel)
            {
                Province province = listProvince.FirstOrDefault(s => s.Code == item.ProvinceCode);

                item.ProvinceName = province?.Name;

                Province ward = listProvince.FirstOrDefault(s => s.Code == item.WardCode);

                item.WardName = ward?.Name;
            }

            return new PagedResultDto<OrderDto>(
               totalCount,
               listDataModel
           );
        }
    }
}
