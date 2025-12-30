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
    [Table("Provinces")]
    public class Province : FullAuditedEntity<long>
    {
        [Required]
        [StringLength(50)]
        public string Code {  get; set; }

        public long? ParentId {  get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}
