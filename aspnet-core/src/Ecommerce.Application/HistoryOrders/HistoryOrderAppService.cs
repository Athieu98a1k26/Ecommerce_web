using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Ecommerce.Authorization.Users;
using Ecommerce.Bases;
using Ecommerce.Common;
using Ecommerce.Entitys;
using Ecommerce.HistoryOrders.Dto;
using Ecommerce.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.HistoryOrders
{
    public interface IHistoryOrderAppService
    {
        Task CreateOrUpdate(CreateUpdateHistoryOrder request);
        Task<PagedResultDto<HistoryOrderDto>> GetPaging(HistoyOrderRequestModel request);
    }
    public class HistoryOrderAppService : EcommerceAppServiceBase
    {
        private readonly IRepository<HistoryOrder,long> _historyRepository;
        private readonly IRepository<Order, long> _orderRepository;

        private readonly IRepository<Person, long> _personRepository;
        public HistoryOrderAppService(IRepository<HistoryOrder, long> historyRepository, IRepository<Order, long> orderRepository, IRepository<Person, long> personRepository)
        {
            _historyRepository = historyRepository;
            _orderRepository = orderRepository;
            _personRepository = personRepository;
        }

        public async Task CreateOrUpdate(CreateUpdateHistoryOrder request)
        {
            HistoryOrder historyOrder = ObjectMapper.Map<HistoryOrder>(request);
            await _historyRepository.InsertAsync(historyOrder);
        }

        [HttpPost]
        public async Task<PagedResultDto<HistoryOrderDto>> GetPaging(HistoyOrderRequestModel request)
        {

            IQueryable<HistoryOrder> query = null;

            if (request.OrderId.HasValue)
            {
                query = _historyRepository.GetAll().Where(s => s.OrderId == request.OrderId);
            }
            else
            {
                User user = await  GetCurrentUserAsync();
                Person person = await _personRepository.GetAll().FirstOrDefaultAsync(s=>s.UserId == user.Id);

                if(person == null)
                {
                    return new PagedResultDto<HistoryOrderDto>(
                       0,
                       null
                   );
                }

                query = _historyRepository.GetAll().Where(s => s.PersonId == person.Id);
            }

            var pagedAndFiltered = query
                    .OrderByDescending(s=>s.CreationTime)
                    .PageBy(request);

            var totalCount = await query.CountAsync();

            var listData = await pagedAndFiltered.ToListAsync();

            var listDataModel = ObjectMapper.Map<List<HistoryOrderDto>>(listData);

            List<long> listOrderId = listDataModel.Select(s=>s.OrderId).ToList();

            var listOrder = await _orderRepository.GetAll().Where(s => listOrderId.Contains(s.Id)).Select(s=>new {s.Id,s.Code}).ToListAsync();

            foreach (var item in listDataModel)
            {
                var order = listOrder.FirstOrDefault(s=>s.Id == item.OrderId);
                if(order == null)
                {
                    continue;
                }

                item.OrderCode = order.Code;
                item.Name = L(item.Action);
            }

            return new PagedResultDto<HistoryOrderDto>(
               totalCount,
               listDataModel
           );
        }

    }
}
