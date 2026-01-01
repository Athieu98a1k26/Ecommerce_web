using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Ecommerce.Entitys;
using Ecommerce.HistoryTransactions.Dto;

namespace Ecommerce.Historys
{
    public interface IHistoryOrderAppService
    {
        Task CreateOrUpdate(CreateUpdateHistoryOrder request);
    }
    public class HistoryOrderAppService : EcommerceAppServiceBase
    {
        private readonly IRepository<HistoryOrder,long> _historyRepository;
        private readonly IRepository<Order, long> _orderRepository;
        public HistoryOrderAppService(IRepository<HistoryOrder, long> historyRepository, IRepository<Order, long> orderRepository)
        {
            _historyRepository = historyRepository;
            _orderRepository = orderRepository;
        }

        public async Task CreateOrUpdate(CreateUpdateHistoryOrder request)
        {
            HistoryOrder historyOrder = ObjectMapper.Map<HistoryOrder>(request);
            await _historyRepository.InsertAsync(historyOrder);
        }
    }
}
