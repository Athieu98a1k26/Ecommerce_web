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
using Ecommerce.Common;
using Abp.Domain.Uow;

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
        private readonly IRepository<Transaction, long> _transactionRepository;
        public OrderAppService(IRepository<Order, long> orderRepository, IRepository<ProductStoreDetail, long> productStoreDetailRepository, IRepository<OrderDetail, long> orderDetailRepository, IRepository<Province, long> provinceRepository,
            IRepository<Transaction, long> transactionRepository)
        {
            _orderRepository = orderRepository;
            _productStoreDetailRepository = productStoreDetailRepository;
            _orderDetailRepository = orderDetailRepository;
            _provinceRepository = provinceRepository;
            _transactionRepository = transactionRepository;
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

        [UnitOfWork]
        public async Task ConfirmedOrder(long orderId)
        {
            // lấy danh sách đơn
            Order order = _orderRepository.Get(orderId);

            if(order == null )
            {
                throw new UserFriendlyException(L("OrderNotFound"));
            }

            if (!IsConfirmed(order.OrderStatus))
            {
                throw new UserFriendlyException(L("ConfirmedNotValid"));
            }

            List<OrderDetail> listOrderDetail = _orderDetailRepository.GetAll().Where(s=>s.OrderId == order.Id).ToList();

            if (order == null)
            {
                throw new UserFriendlyException(L("OrderNotFound"));
            }

            foreach (OrderDetail orderDetail in listOrderDetail)
            {
                await ProcessTransaction(orderDetail);
            }

            //update trạng thái order là đã xác nhận
            order.OrderStatus = OrderStatus.Confirmed;

            await _orderRepository.UpdateAsync(order);
        }

        private bool IsConfirmed(string status)
        {
            if(status == OrderStatus.Confirmed ||
                status == OrderStatus.Cancelled)
            {
                return false;
            }

            return true;
        }

        private bool IsCancelled(string status)
        {
            if (status == OrderStatus.Confirmed ||
                status == OrderStatus.Cancelled)
            {
                return false;
            }

            return true;
        }

        public async Task CancelledOrder(long orderId)
        {
            // lấy danh sách đơn
            Order order = _orderRepository.Get(orderId);

            if (order == null)
            {
                throw new UserFriendlyException(L("OrderNotFound"));
            }

            if (!IsCancelled(order.OrderStatus))
            {
                throw new UserFriendlyException(L("CancelledNotValid"));
            }

            //update trạng thái order là đã xác nhận
            order.OrderStatus = OrderStatus.Cancelled;
            await _orderRepository.UpdateAsync(order);
        }

        private async Task ProcessTransaction(OrderDetail orderDetail)
        {
            try
            {
                List<Transaction> transactions = new List<Transaction>();

                if (string.IsNullOrEmpty(orderDetail.DetailPrice))
                {
                    throw new UserFriendlyException(L("DetailPriceNotFound"));
                }

                //tình toán giá theo từng tháng
                DateTime now = DateTime.Now; // ví dụ 31/12/2025
                int startYear = now.Year;

                string[] parts = orderDetail.DetailPrice.Split(',', StringSplitOptions.RemoveEmptyEntries);

                DateTime previousToDate = now;

                foreach (string part in parts)
                {
                    string[] values = part.Split('-', StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length != 3) continue;

                    int fromMonth = int.Parse(values[0]);
                    int toMonth = int.Parse(values[1]);
                    decimal amount = decimal.Parse(values[2]);

                    int fromYear = startYear;
                    int toYear = toMonth < fromMonth ? startYear + 1 : startYear;

                    // FromDate = ngày hiện tại hoặc ngày sau của khoảng trước
                    DateTime fromDate = previousToDate;
                    DateTime toDate = new DateTime(toYear, toMonth, DateTime.DaysInMonth(toYear, toMonth));

                    Transaction transaction = new Transaction
                    {
                        OrderDetailId = orderDetail.Id,
                        FromDate = fromDate,
                        ToDate = toDate,
                        AmounToBePaid = amount,
                        TranSactionStatus = TransactionOrderStatus.Init
                    };

                    await _transactionRepository.InsertAsync(transaction);

                    // Cập nhật ngày kết thúc của khoảng vừa thêm
                    previousToDate = toDate.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("TransactionError"));
            }
        }
    }
}
