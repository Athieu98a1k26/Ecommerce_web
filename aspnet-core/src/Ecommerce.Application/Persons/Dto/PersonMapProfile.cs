using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Entitys;
using Ecommerce.Orders.Dto;

namespace Ecommerce.Persons.Dto
{
    public class PersonMapProfile:Profile
    {
        public PersonMapProfile()
        {
            CreateMap<CreateUpdatePersonDto, Person>();

            CreateMap<Person, PersonDto>();
        }
    }
}
