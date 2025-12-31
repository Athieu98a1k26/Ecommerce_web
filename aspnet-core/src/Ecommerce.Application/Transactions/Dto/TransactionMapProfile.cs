using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Entity;
using Ecommerce.Entitys;
using Ecommerce.Stores.Dto;

namespace Ecommerce.Transactions.Dto
{
    public class TransactionMapProfile:Profile
    {
        public TransactionMapProfile()
        {
            CreateMap<Transaction, TransactionDto>();
        }
    }
}
