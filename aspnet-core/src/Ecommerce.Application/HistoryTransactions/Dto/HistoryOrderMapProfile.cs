using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Entitys;
using Ecommerce.Historys.Dto;
using Ecommerce.Transactions.Dto;

namespace Ecommerce.HistoryTransactions.Dto
{
    public class HistoryOrderMapProfile:Profile
    {
        public HistoryOrderMapProfile()
        {
            CreateMap<HistoryOrder, HistoryOrderDto>();
            CreateMap<CreateUpdateHistoryOrder, HistoryOrder>();
        }
    }
}
