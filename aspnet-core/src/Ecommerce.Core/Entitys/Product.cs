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
    [Table("Products")]
    public class Product : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(100)]
        public string Code {  get; set; }

        [Required]
        [MaxLength(250)]
        public string Name {  get; set; }

    }
}
