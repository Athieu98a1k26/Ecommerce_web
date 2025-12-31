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
        public string FullName { get; set; }
        public long PersonId { get; set; } //Id người dùng

        public string PhoneNumber {  get; set; }
        public string Email {  get; set; }

        public string ProvinCode { get; set; }
        public string ProvinName { get; set; }
        public string WardCode { get; set; }
        public string WardName { get; set; }
        public string Note { get; set; } // ghi chú

        public string DeliveryMethod { get; set; } // hình thức nhận hàng

        public string OrderStatus { get; set; } // trạng thái đơn hàng
    }
}
