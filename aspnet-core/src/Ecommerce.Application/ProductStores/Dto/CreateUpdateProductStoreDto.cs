using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ProductStores.Dto
{
    public class CreateUpdateProductStoreDto
    {
        public long? Id { get; set; }
        public string StoreCode { get; set; }
        public string ProductCode { get; set; }

        public string PathImage { get; set; } // ảnh sản phẩm
        public decimal Price { get; set; } // giá sản phẩm thấp nhất

        public float? Stars { get; set; } // số sao đánh giá

        public int? Sold { get; set; } // số sản phẩm đã bán
        public int Count {  get; set; } // tổng sản phẩm
        public List<ProductStoreDetailDto> ListProductStoreDetailDto { get; set; }
    }
}
