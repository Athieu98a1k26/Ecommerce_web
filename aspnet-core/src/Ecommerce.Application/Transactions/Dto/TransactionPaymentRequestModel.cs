using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Transactions.Dto
{
    public class TransactionPaymentRequestModel
    {
        public long? TransactionId { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
