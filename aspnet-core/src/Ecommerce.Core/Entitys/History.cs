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
    [Table("Historys")]
    public class History : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(100)]
        public string Action {  get; set; }

        [Required]
        [MaxLength(500)]
        public string Note {  get; set; }


        [Required]
        public long PersonId {  get; set; }
    }
}
