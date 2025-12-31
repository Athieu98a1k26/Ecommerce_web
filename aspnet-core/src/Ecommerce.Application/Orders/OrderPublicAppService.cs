using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Ecommerce.Bases;
using Ecommerce.Common;
using Ecommerce.Entitys;
using Ecommerce.Helper;
using Ecommerce.Orders.Dto;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Orders
{
    public interface IOrderPublicAppService
    {
        Task CreateOrEdit(CreateUpdateOrderDto request);
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
            order.OrderStatus = CatalogType.OrderStatus.First();
            long id = await _orderRepository.InsertAndGetIdAsync(order);

            //thêm vào bảng detail
            List<ProductStoreDetail> listProductStoreDetail = await _productStoreDetailRepository.GetAll().Where(s => listProductStoreDetailId.Contains(s.Id)).ToListAsync();
            foreach (var item in request.ListOrderDetailDto)
            {
                ProductStoreDetail productStoreDetail1 = listProductStoreDetail.FirstOrDefault(s => s.Id == item.ProductStoreDetailId);

                OrderDetail orderDetail = ObjectMapper.Map<OrderDetail>(item);
                orderDetail.OrderId = id;
                orderDetail.ProductStoreId = productStoreDetail1?.ProductStoreId ?? 0;

                orderDetail.OrderDetailStatus = CatalogType.OrderDetailStatus.First();

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
    }
}
