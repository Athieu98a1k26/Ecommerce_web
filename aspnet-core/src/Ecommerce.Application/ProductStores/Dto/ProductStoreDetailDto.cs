using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ProductStores.Dto
{
    public class ProductStoreDetailDto
    {
        public long Id { get; set; }
        public long? ProductStoreId { get; set; }

        public string PathImage { get; set; } // ảnh

        public string CapacityCode { get; set; } // dung lượng

        public string ColorCode { get; set; } // màu sắc

        public string MachineConditionCode { get; set; }  // tình trạng máy

        // gói trả
        public string PackageCode { get; set; }

        public long LeaseTermCode { get; set; } // thời hạn thuê

        public decimal Prepay { get; set; } // trả trước

        public decimal Price { get; set; } // giá của sản phẩm theo phiên bản, màu sắc
        public string DetailPrice { get; set; }
        public int Count { get; set; } // tổng sản phẩm đang có

        public bool IsActive {  get; set; } // biến dùng trên angular

        public string ProductName {  get; set; }
        public string ProductCode {  get; set; }

        public string StoreName {  get; set; }
        public string StoreCode {  get; set; }
    }
}
