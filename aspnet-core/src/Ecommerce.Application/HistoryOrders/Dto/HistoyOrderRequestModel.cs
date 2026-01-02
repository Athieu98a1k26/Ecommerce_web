using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Bases;

namespace Ecommerce.HistoryOrders.Dto
{
    public class HistoyOrderRequestModel:BaseRequest
    {
        public long? OrderId {  get; set; }
    }
}
