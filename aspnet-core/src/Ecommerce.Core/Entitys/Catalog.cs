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
    [Table("Catalogs")]
    public class Catalog : FullAuditedEntity<long>
    {
        [Required]
        [StringLength(100)]
        public string Code {  get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string CatalogType { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }
}
