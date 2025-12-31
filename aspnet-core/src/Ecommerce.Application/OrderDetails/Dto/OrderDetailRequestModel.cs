using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Bases;

namespace Ecommerce.OrderDetails.Dto
{
    public class OrderDetailRequestModel:BaseRequest
    {
        public long? OrderId { get; set; }
    }
}
