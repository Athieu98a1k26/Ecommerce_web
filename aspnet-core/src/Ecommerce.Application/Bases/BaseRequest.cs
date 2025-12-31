using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Ecommerce.Bases
{
    public class BaseRequest: PagedAndSortedResultRequestDto
    {
        public string? Search {  get; set; }
        public string? StoreCode { get; set; }
    }
}
