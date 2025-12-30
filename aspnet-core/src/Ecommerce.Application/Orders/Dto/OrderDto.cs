using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Orders.Dto
{
    public class OrderDto
    {
        public long? Id { get; set; }

        public string Code { get; set; }

        public long ProductStoreDetailId { get; set; }

        public long ProductStoreId { get; set; }

        public int Count { get; set; }  // số lượng

        public long PersonId { get; set; } //Id người dùng

        public string Note { get; set; } // ghi chú

        public string DeliveryMethod { get; set; } // hình thức nhận hàng

        public string OrderStatus { get; set; }
    }
}
