using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.OrderDetails.Dto
{
    public class OrderDetailDto
    {
        public long? Id { get; set; }
        public long? OrderId { get; set; }

        public long ProductStoreDetailId { get; set; }

        public long ProductStoreId { get; set; }

        public string ProductName {  get; set; }

        public string StoreName { get; set; }

        public int Count { get; set; }  // số lượng

        public decimal Price { get; set; } // số tiền
        public string DetailPrice { get; set; } // gen ngày tháng
    }
}
