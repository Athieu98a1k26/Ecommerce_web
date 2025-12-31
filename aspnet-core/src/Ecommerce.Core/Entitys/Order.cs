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
        public int Count { get; set; }

        [Required]
        public decimal Price { get; set; } // tổng tiền đơn hàng

        [Required]
        public long PersonId {  get; set; } //Id người dùng

        [Required]
        [StringLength(255)]
        public string FullName {  get; set; }

        [Required]
        [StringLength(100)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string ProvinceCode {  get; set; }

        [StringLength(100)]
        public string WardCode {  get; set; }

        [StringLength(512)]
        public string Address {  get; set; }

        [StringLength(255)]       
        public string Note {  get; set; } // ghi chú

        [StringLength(50)]
        public string DeliveryMethod {  get; set; } // hình thức nhận hàng

        [StringLength(50)]
        public string OrderStatus {  get; set; } // trạng thái đơn hang
    }
}
