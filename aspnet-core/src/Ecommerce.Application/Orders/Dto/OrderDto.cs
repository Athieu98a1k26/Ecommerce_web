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

        public int Count { get; set; }

        public decimal Price { get; set; } // tổng tiền đơn hàng

        public long PersonId { get; set; } //Id người dùng

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }

        public string WardCode { get; set; }
        public string WardName { get; set; }

        public string Address { get; set; }

        public string Note { get; set; } // ghi chú

        public string DeliveryMethod { get; set; } // hình thức nhận hàng

        public string OrderStatus { get; set; } // trạng thái đơn hàng

        public bool IsExpanded {  get; set; }
    }
}
