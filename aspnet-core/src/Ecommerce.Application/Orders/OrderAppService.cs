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
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Abp.UI;
using Ecommerce.Entitys;
using Ecommerce.Orders.Dto;

namespace Ecommerce.Orders
{
    public interface IOrderAppService
    {
        Task<PagedResultDto<OrderDto>> GetPaging(BaseRequest baseRequest);
    }

    [AbpAuthorize(PermissionNames.Pages_Orders)]
    public class OrderAppService : EcommerceAppServiceBase, IOrderAppService
    {
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<ProductStoreDetail, long> _productStoreDetailRepository;
        private readonly IRepository<Province, long> _provinceRepository;
        private readonly IRepository<OrderDetail, long> _orderDetailRepository;
        public OrderAppService(IRepository<Order, long> orderRepository, IRepository<ProductStoreDetail, long> productStoreDetailRepository, IRepository<OrderDetail, long> orderDetailRepository, IRepository<Province, long> provinceRepository)
        {
            _orderRepository = orderRepository;
            _productStoreDetailRepository = productStoreDetailRepository;
            _orderDetailRepository = orderDetailRepository;
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
                listProvinceCode.Add(item.ProvinceCode);
                listProvinceCode.Add(item.WardCode);
            }    

            List<Province> listProvince = await _provinceRepository.GetAll().Where(s=> listProvinceCode.Contains(s.Code)).ToListAsync();

            foreach (var item in listDataModel)
            {
                Province province = listProvince.FirstOrDefault(s=>s.Code == item.ProvinceCode);

                item.ProvinceName = province?.Name;

                Province ward = listProvince.FirstOrDefault(s=>s.Code == item.WardCode);

                item.WardName = ward?.Name;
            }

            return new PagedResultDto<OrderDto>(
               totalCount,
               listDataModel
           );
        }

        public async Task ConfirmedOrder(long orderId)
        {
            // lấy danh sách đơn
            Order order = _orderRepository.Get(orderId);

            if(order == null )
            {
                throw new UserFriendlyException(L("OrderNotFound"));
            }    

            List<OrderDetail> listOrderDetail = _orderDetailRepository.GetAll().Where(s=>s.OrderId == order.Id).ToList();

            if (order == null)
            {
                throw new UserFriendlyException(L("OrderNotFound"));
            }

            foreach (OrderDetail orderDetail in listOrderDetail)
            {

            }
        }

        

    }
}
