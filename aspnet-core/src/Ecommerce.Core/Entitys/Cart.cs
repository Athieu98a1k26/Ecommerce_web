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
    [Table("Carts")]
    public class Cart : FullAuditedEntity<long>
    {
        [Required]
        public long PersonId {  get; set; }  // người dùng nào

        [Required]
        public int Quantity {  get; set; } // số lượng bao nhiêu

        [Required]
        public long ProductStoreDetailId {  get; set; } // sản phẩm gì
    }
}
