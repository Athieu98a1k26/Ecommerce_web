using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Authorization.Roles;
using Ecommerce.Entitys;
using Ecommerce.Roles.Dto;

namespace Ecommerce.Products.Dto
{
    public class ProductMapProfile : Profile
    {
        public ProductMapProfile()
        {
            CreateMap<CreateUpdateProductDto, Product>();

            CreateMap<Product, ProductDto>();
        }
    }
}
