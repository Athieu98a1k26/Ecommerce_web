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
    [Table("Persons")]
    public class Person : FullAuditedEntity<long>
    {
        [StringLength(255)]
        public string FullName {  get; set; }

        [StringLength(100)]
        public string PhoneNumber {  get; set; }

        [StringLength(100)]
        public string Email {  get; set; }

        [Required]
        public long UserId {  get; set; }
    }
}
