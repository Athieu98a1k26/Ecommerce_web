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
    [Table("Catalogs")]
    public class Catalog : FullAuditedEntity<long>
    {
        [Required]
        public string Code {  get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CatalogType { get; set; }

        public string Description { get; set; }
    }
}
