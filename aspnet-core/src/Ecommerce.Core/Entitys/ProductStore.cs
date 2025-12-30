using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace Ecommerce.Entitys
{
    [Table("ProductStores")]
    public class ProductStore : FullAuditedEntity<long>
    {
        [Required]
        [StringLength(100)]
        public string StoreCode {  get; set; }

        [Required]
        [StringLength(100)]
        public string ProductCode { get; set; }

        [Required]
        public decimal Price {  get; set; } // giá sản phẩm thấp nhất

        public float? Stars { get; set; } // số sao đánh giá

        public int? Sold { get; set; } // số sản phẩm đã bán
    }
}
