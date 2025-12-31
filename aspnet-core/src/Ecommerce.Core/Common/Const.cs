using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Common
{
    public static class Capacity
    {
        public static string Capacity64GB = "64GB";
        public static string Capacity128GB = "128GB";
        public static string Capacity256GB = "256GB";
        public static string Capacity512GB = "512GB";
    }

    public static class Color
    {
        public static string Black = "Black";
        public static string White = "White";
        public static string Blue = "Blue";
        public static string Red = "Red";
    }

    public static class Condition
    {
        public static string New = "New";
        public static string Used = "Used";
    }

    public static class PackageCode
    {
        public static string RentThenBuy = "RentThenBuy";
    }

    public static class LeaseTermCode
    {
        public static int Month1 = 1;
        public static int Month2 = 2;
        public static int Month3 = 3;
        public static int Month4 = 4;
        public static int Month5 = 5;
        public static int Month6 = 6;
        public static int Month7 = 7;
        public static int Month8 = 8;
        public static int Month9 = 9;
        public static int Month10 = 10;
        public static int Month11 = 11;
        public static int Month12 = 12;
    }

    public static class DeliveryMethod
    {
        public static string InStore = "InStore";
        public static string HomeDelivery = "HomeDelivery";
    }

    public static class OrderStatus
    {
        public static string Init = "Init";
        public static string Confirmed = "Confirmed";
        public static string Cancelled = "Cancelled";
    }

    public static class TransactionStatus
    {
        public static string Init = "Init";
        public static string Processing = "Processing";
        public static string Paid = "Paid";
        public static string Reject = "Reject";
    }
}

    
    
