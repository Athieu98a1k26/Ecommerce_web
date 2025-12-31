using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.OrderDetails.Dto;

namespace Ecommerce.Orders.Dto
{
    public class CreateUpdateOrderDto
    {

        public long PersonId { get; set; } //Id người dùng

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string ProvinceCode { get; set; }

        public string WardCode { get; set; }

        public string Address { get; set; }

        public string Note { get; set; } // ghi chú

        public string DeliveryMethod { get; set; } // hình thức nhận hàng

        public List<OrderDetailDto> ListOrderDetailDto {  get; set; }

    }
}
