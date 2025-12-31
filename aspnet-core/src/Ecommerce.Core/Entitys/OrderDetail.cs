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
    [Table("OrderDetails")]
    public class OrderDetail : FullAuditedEntity<long>
    {
        [Required]
        public long? OrderId { get; set; }

        [Required]
        public long ProductStoreDetailId { get; set; }

        [Required]
        public long ProductStoreId { get; set; }

        [Required]
        public int Count { get; set; }  // số lượng

        [Required]
        public decimal Price { get; set; } // số tiền.

        [Required]
        [StringLength(255)]
        public string DetailPrice { get; set; } // dữ liệu giá tiền theo từng tháng

        [StringLength(100)]
        public string OrderDetailStatus { get; set; } // Trạng thái
    }
}
