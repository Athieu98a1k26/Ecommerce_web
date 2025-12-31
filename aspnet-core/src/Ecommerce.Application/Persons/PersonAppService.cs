using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.UI;
using Ecommerce.Authorization.Users;
using Ecommerce.Entitys;
using Ecommerce.Orders;
using Ecommerce.Persons.Dto;

namespace Ecommerce.Persons
{
    public interface IPersonAppService
    {
        Task UpdatePerson(CreateUpdatePersonDto input);
        Task<PersonDto> GetPerson(long userId);
    }
    public class PersonAppService : EcommerceAppServiceBase, IPersonAppService
    {
        private readonly IRepository<Person, long> _personRepository;

        public PersonAppService(IRepository<Person, long> personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonDto> GetPerson(long userId)
        {
            Person person = _personRepository.GetAll().FirstOrDefault(s=>s.UserId == userId);

            if (person == null)
            {
                return null;
            }

            return ObjectMapper.Map<PersonDto>(person);
        }

        public async Task UpdatePerson(CreateUpdatePersonDto input)
        {
            if (!input.Id.HasValue)
            {
                throw new UserFriendlyException(L("PersonNotFound"));
            }

            Person person = _personRepository.Get(input.Id??0);

            if (person ==null)
            {
                throw new UserFriendlyException(L("PersonNotFound"));
            }

            person.FullName = input.FullName;
            person.PhoneNumber = input.PhoneNumber;
            person.Email = input.Email;

            _personRepository.Update(person);
        }
    }
}
