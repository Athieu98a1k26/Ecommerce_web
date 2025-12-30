using System;
using System.Collections.Generic;
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
        public string OrderCode {  get; set; } // mã đơn hàng
        public DateTime Time {  get; set; }  // tháng
        public decimal AmounToBePaid {  get; set; } // số tiêng phải trả

        public string Status {  get; set; } // Trạng thái
    }
}
