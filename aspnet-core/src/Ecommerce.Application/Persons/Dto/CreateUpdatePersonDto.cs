using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persons.Dto
{
    public class CreateUpdatePersonDto
    {
        public long? Id { get; set; }
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public long UserId { get; set; }
    }
}
