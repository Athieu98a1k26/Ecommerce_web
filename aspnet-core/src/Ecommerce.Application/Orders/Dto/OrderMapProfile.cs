using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Entitys;
using Ecommerce.Products.Dto;

namespace Ecommerce.Orders.Dto
{
    public class OrderMapProfile:Profile
    {
        public OrderMapProfile()
        {
            CreateMap<CreateUpdateOrderDto, Order>();

            CreateMap<OrderDetailDto, OrderDetail>();

            CreateMap<Order, OrderDto>();
        }
    }
}
