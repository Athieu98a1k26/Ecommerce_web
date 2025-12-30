using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Ecommerce.Common;
using Ecommerce.Entitys;
using Ecommerce.Helper;
using Ecommerce.Orders.Dto;
using Ecommerce.Products.Dto;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Orders
{
    public interface IOrderAppService
    {

    }
    public class OrderAppService : EcommerceAppServiceBase, IOrderAppService
    {
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<ProductStoreDetail, long> _productStoreDetailRepository;
        private readonly IRepository<ProductStore, long> _productStoreRepository;
        public OrderAppService(IRepository<Order, long> orderRepository, IRepository<ProductStoreDetail, long> productStoreDetailRepository, IRepository<ProductStore, long> productStoreRepository)
        {
            _orderRepository = orderRepository;
            _productStoreDetailRepository = productStoreDetailRepository;
            _productStoreRepository = productStoreRepository;
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

            // gen detail

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
