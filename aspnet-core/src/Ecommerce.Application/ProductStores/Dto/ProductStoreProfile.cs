using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Entitys;
using Ecommerce.Products.Dto;

namespace Ecommerce.ProductStores.Dto
{
    public class ProductStoreProfile:Profile
    {
        public ProductStoreProfile()
        {
            CreateMap<ProductStoreDetailDto, ProductStoreDetail>();
            CreateMap<ProductStoreDetail, ProductStoreDetailDto>();
            CreateMap<ProductStore, ProductStoreDto>();

            CreateMap<CreateUpdateProductStoreDto, ProductStore>();
        }
    }
}
