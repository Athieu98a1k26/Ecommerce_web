using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Entity;
using Ecommerce.Entitys;
using Ecommerce.Products.Dto;

namespace Ecommerce.Stores.Dto
{
    public class StoreMapProfile:Profile
    {
        public StoreMapProfile()
        {
            CreateMap<CreateUpdateStoreDto, Store>();

            CreateMap<Store, StoreDto>();
        }
    }
}
