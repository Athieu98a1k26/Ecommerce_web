using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Common
{
    public static class CatalogType
    {
        public static HashSet<string> Capacitys = new HashSet<string>
        {
            "64GB",
            "128GB",
            "256GB",
            "512GB"
        };

        public static HashSet<string> Colors = new HashSet<string>
        {
            "Black",
            "White",
            "Blue",
            "Red"
        };

        public static HashSet<string> Conditions = new HashSet<string>
        {
            "New",
            "Used",
            "Refurbished"
        };

        //
        public static HashSet<string> PackageCodes = new HashSet<string>
        {
            "RentThenBuy",
        };

        // Package Codes (1 month to 12 months)
        public static readonly Dictionary<int, string> LeaseTermCodes = new Dictionary<int, string>
        {
            { 1, "1Month" },
            { 2, "2Months" },
            { 3, "3Months" },
            { 4, "4Months" },
            { 5, "5Months" },
            { 6, "6Months" },
            { 7, "7Months" },
            { 8, "8Months" },
            { 9, "9Months" },
            { 10, "10Months" },
            { 11, "11Months" },
            { 12, "12Months" }
        };

        // hình thức nhận hàng
        public static HashSet<string> DeliveryMethods = new HashSet<string>
        {
            "InStore",       // Nhận tại cửa hàng
            "HomeDelivery"   // Giao hàng tận nhà
        };

        public static HashSet<string> OrderStatus = new HashSet<string>
        {
            "Pending",       // Mới tạo
            "Confirmed",   // Xác nhận 
            "Reject"   // Hủy
        };

        public static HashSet<string> OrderDetailStatus = new HashSet<string>
        {
            "WaitingForPayment",       // Mới tạo
            "Paid",   // Xác nhận 
            "Reject"   // Hủy
        };


    }
}

    
    
