using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace Ecommerce.Entity
{
    [Table("ProductStoreDetails")]
    public class ProductStoreDetail : FullAuditedEntity<long>
    {
        [Required]
        public string ProductCode {  get; set; }
        
        [Required]
        public string PathImage {  get; set; }

        public string CapacityCode {  get; set; } // dung lượng

        public string ColorCode {  get; set; } // màu sắc

        //tình trạng máy
        [Required]
        public string MachineConditionCode {  get; set; }  // tình trạng máy

        // gói trả
        [Required]
        public long PackageCode {  get; set; }

        [Required]
        public string LeaseTermCode { get; set; } // thời hạn thuê

        [Required]
        public decimal Prepay { get; set; } // trả trước

        [Required]
        public decimal Price { get; set; } // giá của sản phẩm theo phiên bản, màu sắc
    }
}
