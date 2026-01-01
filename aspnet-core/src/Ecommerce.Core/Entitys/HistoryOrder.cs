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
    [Table("HistoryOrders")]
    public class HistoryOrder : FullAuditedEntity<long>
    {
        [Required]
        [StringLength(100)]
        public string Action {  get; set; }

        [Required]
        [StringLength(500)]
        public string Note {  get; set; }

        [Required]
        public long OrderId {  get; set; }

        public long? TransactionId { get; set; }
    }
}
