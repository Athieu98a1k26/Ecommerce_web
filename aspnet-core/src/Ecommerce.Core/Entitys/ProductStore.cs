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
        [MaxLength(100)]
        public string StoreCode {  get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductCode { get; set; }
    }
}
