using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace Ecommerce.Entity
{
    [Table("ProductStores")]
    public class ProductStore : FullAuditedEntity<long>
    {
        [Required]
        public string ShopCode {  get; set; }

        [Required]
        public string ProductCode { get; set; }
    }
}
