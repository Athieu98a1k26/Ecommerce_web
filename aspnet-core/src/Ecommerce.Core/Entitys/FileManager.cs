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
    [Table("FileManagers")]
    public class FileManager : FullAuditedEntity<long>
    {
        [Required]
        [StringLength(255)]
        public string Path {  get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Extension {  get; set; }

        [Required]
        [StringLength(255)]
        public string Subsystem {  get; set; }
    }
}
