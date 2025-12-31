using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Bases;

namespace Ecommerce.Transactions.Dto
{
    public class TransactionRequestModel:BaseRequest
    {
        public long? OrderDetailId { get; set; }
    }
}
