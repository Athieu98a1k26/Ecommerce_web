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
    [Table("Transactions")]
    public class Transaction : FullAuditedEntity<long>
    {
        [Required]
        public long OrderDetailId { get; set; } // chi tiết đơn hàng
        public DateTime FromDate { get; set; }  // bắt đầu
        public DateTime ToDate { get; set; } // kết thúc

        [Required]
        public decimal AmounToBePaid { get; set; } // số tiêng phải trả

        [StringLength(100)]
        public string TranSactionStatus { get; set; } // Trạng thái

        [StringLength(50)]
        public string StatuApprove { get; set; } // trạng thái phê duyệt của admin, nhân viên

        [StringLength(50)]
        public string Reason { get; set; } // lý do

        [StringLength(255)]
        public string FileId {  get; set; } // danh sách file
    }
}
