using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Ecommerce.Entitys;
using Ecommerce.MultiTenancy;

namespace Ecommerce.Products.Dto
{
    public class CreateUpdateProductDto
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
