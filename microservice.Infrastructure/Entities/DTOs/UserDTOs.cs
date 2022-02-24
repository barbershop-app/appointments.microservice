using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microservice.Infrastructure.Entities.DTOs
{
    public class UserDTOs
    {
        public class Create
        {
            public string PhoneNumber { get; set; }
        }

        public class Update : Create
        {
            public Guid Id { get; set; }
        }

    }
}
