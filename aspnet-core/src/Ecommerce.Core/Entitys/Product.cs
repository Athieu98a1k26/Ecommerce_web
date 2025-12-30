using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;

namespace Ecommerce.Entitys
{
    [Table("Products")]
    [AutoMapTo(typeof(Product))]
    public class Product : FullAuditedEntity<long>
    {
        [Required]
        [StringLength(100)]
        public string Code {  get; set; }

        [Required]
        [StringLength(255)]
        public string Name {  get; set; }

    }
}
