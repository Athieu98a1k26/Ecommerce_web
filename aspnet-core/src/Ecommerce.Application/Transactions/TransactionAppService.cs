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
using Microsoft.AspNetCore.Http;
using Ecommerce.FileManagers;
using Abp.UI;
using Ecommerce.Common;

namespace Ecommerce.Transactions
{
    public interface ITransactionAppService
    {
        Task<PagedResultDto<TransactionDto>> GetPaging(TransactionRequestModel request);
        Task PaymentWithEvidence([FromForm] TransactionPaymentRequestModel request);
    }
    public class TransactionAppService : EcommerceAppServiceBase, ITransactionAppService
    {
        private readonly IRepository<Transaction, long> _transactionRepository;
        private readonly FileMangerAppService _fileMangerAppService;

        public TransactionAppService(IRepository<Transaction, long> transactionRepository, FileMangerAppService fileMangerAppService)
        {
            _transactionRepository = transactionRepository;
            _fileMangerAppService = fileMangerAppService;
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

        [DisableRequestSizeLimit]
        public async Task PaymentWithEvidence([FromForm] TransactionPaymentRequestModel request)
        {
            if(!request.TransactionId.HasValue)
            {
                throw new UserFriendlyException(L("TransactionNotFound"));
            }    

            Transaction transaction = _transactionRepository.Get(request.TransactionId.Value);

            if(transaction == null)
            {
                throw new UserFriendlyException(L("TransactionNotFound"));
            }    

            if(transaction.TranSactionStatus == TransactionOrderStatus.AwaitingApproval)
            {
                throw new UserFriendlyException(L("TransactionNotWorking"));
            }

            List<long> listId = await _fileMangerAppService.UploadFilesAsync(request.Files, SubSystem.Transaction);

            transaction.TranSactionStatus = TransactionOrderStatus.AwaitingApproval;
            transaction.FileId = string.Join(",", listId);
        }
    }
}
