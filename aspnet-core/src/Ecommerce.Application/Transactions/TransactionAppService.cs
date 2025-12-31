using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Ecommerce.Bases;
using Ecommerce.Entitys;
using Ecommerce.Stores;
using Ecommerce.Stores.Dto;
using Ecommerce.Transactions.Dto;
using Microsoft.AspNetCore.Mvc;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Transactions
{
    public interface ITransactionAppService
    {
        Task<PagedResultDto<TransactionDto>> GetPaging(TransactionRequestModel request);
    }
    public class TransactionAppService : EcommerceAppServiceBase, ITransactionAppService
    {
        private readonly IRepository<Transaction, long> _transactionRepository;

        public TransactionAppService(IRepository<Transaction, long> transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpPost]
        public async Task<PagedResultDto<TransactionDto>> GetPaging(TransactionRequestModel request)
        {
            var query = _transactionRepository.GetAll().Where(s => s.OrderDetailId == request.OrderDetailId);

            var pagedAndFiltered = query
                .OrderBy(s=>s.FromDate)
                .PageBy(request);

            var totalCount = await query.CountAsync();

            var listData = await pagedAndFiltered.ToListAsync();

            var listDataModel = ObjectMapper.Map<List<TransactionDto>>(listData);

            return new PagedResultDto<TransactionDto>(
               totalCount,
               listDataModel
            );
        }
    }
}
