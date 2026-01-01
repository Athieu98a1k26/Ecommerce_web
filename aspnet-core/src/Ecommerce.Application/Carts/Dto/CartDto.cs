using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Carts.Dto
{
    public class CartDto
    {
        public long Id {  get; set; }
        public long PersonId { get; set; }  // người dùng nào

        public int Quantity { get; set; } // số lượng bao nhiêu

        public long ProductStoreDetailId { get; set; } // sản phẩm gì
        public string ProductName {  get; set; }
        public string ProductCode {  get; set; }
        public string PathImage {  get; set; }
        public decimal Price {  get; set; }

        public bool InStock {  get; set; }
        public bool Selected {  get; set; }
    }
}
