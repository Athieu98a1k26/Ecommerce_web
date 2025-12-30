using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace Ecommerce.Entitys
{
    [Table("ProductStoreDetails")]
    public class ProductStoreDetail : FullAuditedEntity<long>
    {
        [Required]
        public long ProductStoreId {  get; set; }
        
        [Required]
        [StringLength(255)]
        public string PathImage {  get; set; }

        [StringLength(100)]
        public string CapacityCode {  get; set; } // dung lượng

        [StringLength(100)]
        public string ColorCode {  get; set; } // màu sắc

        //tình trạng máy
        [StringLength(100)]
        public string MachineConditionCode {  get; set; }  // tình trạng máy

        // gói trả
        [Required]
        [StringLength(100)]
        public string PackageCode {  get; set; }

        [Required]
        public int LeaseTermCode { get; set; } // thời hạn thuê

        [Required]
        public decimal Prepay { get; set; } // trả trước ex:50%

        [Required]
        public decimal Price { get; set; } // giá của sản phẩm theo phiên bản, màu sắc

        [Required]
        [StringLength(255)]
        public string DetailPrice {  get; set; } // chi tiết giá theo từng tháng

        [Required]
        public int Count {  get; set; } // số lượng sản phẩm
    }
}
