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
    [Table("Orders")]
    public class Order : FullAuditedEntity<long>
    {
        [Required]
        [StringLength(100)]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        public string ShopCode {  get; set; } // cửa hàng

        [Required]
        [StringLength(100)]
        public string ProductStore {  get; set; } // sản phẩm thuộc cửa hàng

        [Required]
        public int Count {  get; set; }  // số lượng

        [Required]
        public long PersonId {  get; set; } //Id người dùng

        [StringLength(255)]       
        public string Note {  get; set; } // ghi chú

        [StringLength(50)]
        public string DeliveryMethod {  get; set; } // hình thức nhận hàng

        [StringLength(50)]
        public string OrderStatus {  get; set; } // trạng thái đơn hàng
    }
}
