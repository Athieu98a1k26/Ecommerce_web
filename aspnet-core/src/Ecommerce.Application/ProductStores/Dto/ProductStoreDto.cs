using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ProductStores.Dto
{
    public class ProductStoreDto
    {
        public long Id {  get; set; }
        public string StoreCode { get; set; }
        public string StoreName {  get; set; }
        public string ProductCode { get; set; }
        public string ProductName {  get; set; }

        public string PathImage { get; set; }
        public decimal? Price {  get; set; } // giá sản phẩm
        public float? Stars { get; set; } // số sao đánh giá
        public int? Sold {  get; set; } // số sản phẩm đã bán

        public List<ProductStoreDetailDto> ListProductStoreDetailDto {  get; set; }
    }
}
