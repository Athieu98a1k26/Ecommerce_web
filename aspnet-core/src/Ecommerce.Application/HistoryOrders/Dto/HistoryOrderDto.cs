using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.HistoryOrders.Dto
{
    public class HistoryOrderDto
    {
        public long Id {  get; set; }
        public string Action { get; set; }
        public string Note { get; set; }
        public long OrderId { get; set; }
        public string OrderCode { get; set; }
        public long? TransactionId { get; set; }
        public long PersonId { get; set; }
        public string? Name { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
