using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Entitys;

namespace Ecommerce.OrderDetails.Dto
{
    internal class OrderDetailMapProfile:Profile
    {
        public OrderDetailMapProfile() {
            CreateMap<OrderDetailDto, OrderDetail>().ReverseMap();
        }
    }
}
