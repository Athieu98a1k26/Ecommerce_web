using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.FileManagers.Dto;

namespace Ecommerce.Transactions.Dto
{
    public class TransactionDto
    {
        public long Id {  get; set; }
        public long OrderDetailId { get; set; } // chi tiết đơn hàng
        public DateTime FromDate { get; set; }  // bắt đầu
        public DateTime ToDate { get; set; } // kết thúc

        public decimal AmounToBePaid { get; set; } // số tiêng phải trả

        public string TranSactionStatus { get; set; } // Trạng thái

        public string Reason { get; set; }

        public List<FileManagerDto> ListFile {  get; set; }

        public List<long> ListFileId { get; set; }
    }
}
