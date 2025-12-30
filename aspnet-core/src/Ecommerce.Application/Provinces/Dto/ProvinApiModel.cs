using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Provinces.Dto
{
    public class ProvinApiModel
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string AdministrativeLevel { get; set; } = default!;
        public string ProvinceCode { get; set; } = default!;
        public string ProvinceName { get; set; } = default!;
    }

    public class ApiResponse
    {
        public string RequestId { get; set; } = default!;
        public List<ProvinApiModel> provinces { get; set; } = new();
        public List<ProvinApiModel> communes { get; set; } = new();
    }
}
