using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Entitys;
using Ecommerce.OrderDetails.Dto;

namespace Ecommerce.Carts.Dto
{
    public class CartMapProfile:Profile
    {
        public CartMapProfile()
        {
            CreateMap<CreateUpdateCartDto, Cart>();
            CreateMap<Cart, CartDto>();
        }
    }
}
