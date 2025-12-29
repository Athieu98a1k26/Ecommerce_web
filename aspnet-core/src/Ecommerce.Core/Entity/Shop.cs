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
    [Table("Shops")]
    public class Shop : FullAuditedEntity<long>
    {
        [Required]
        public string Code {  get; set; }

        [Required]
        public string Name { get; set; }

    }
}
