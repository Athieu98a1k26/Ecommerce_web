using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Helper
{
    public static class OrderHelper
    {
        public static string GenerateOrderCode()
        {
            return $"DH{DateTime.Now:yyyyMMddHHmmss}{Guid.NewGuid():N}"[..26].ToUpper();
        }
    }
}
