using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Entitys;
using Ecommerce.Products.Dto;

namespace Ecommerce.Provinces.Dto
{
    public class ProvinceMapProfile:Profile
    {
        public ProvinceMapProfile()
        {
            CreateMap<Province, ProvinceDto>();
        }
    }
}
