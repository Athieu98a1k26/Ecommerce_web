using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Bases;

namespace Ecommerce.Persons.Dto
{
    public class OrderRequestDto: BaseRequest
    {
        public string PhoneNumber {  get; set; }
        public string Email {  get; set; }
        public string OrderStatus {  get; set; }
    }
}
