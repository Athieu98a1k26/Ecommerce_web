using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Ecommerce.Entity;
using Ecommerce.Entitys;

namespace Ecommerce.Stores.Dto
{
    public class CreateUpdateStoreDto
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }
    }
}
