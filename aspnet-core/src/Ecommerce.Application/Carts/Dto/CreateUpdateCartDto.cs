using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Carts.Dto
{
    public class CreateUpdateCartDto
    {
        public int? Quantity { get; set; } // số lượng bao nhiêu

        public long? ProductStoreDetailId { get; set; } // sản phẩm gì
    }
}
