using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.HistoryTransactions.Dto
{
    public class CreateUpdateHistoryOrder
    {
        public string Action { get; set; }
        public string Note { get; set; }
        public long OrderId { get; set; }
        public long? TransactionId { get; set; }

        public string? Name {  get; set; }
    }
}
