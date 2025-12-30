using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Orders.Dto
{
    public class CreateUpdateOrderDto
    {
        public long? ProductStoreDetailId {  get; set; }
        public int? Count {  get; set; }
        public string Note {  get; set; }
        public string DeliveryMethod { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber {  get; set; }
        public string Email { get; set; }

        public string ProvinCode {  get; set; }
        public string WardCode {  get; set; }
    }
}
