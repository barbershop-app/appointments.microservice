using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microservice.Infrastructure.Entities.DB
{
    public class User
    {
        public Guid Id { get; set; }
         
        [StringLength(10)]
        public string PhoneNumber { get; set; }
    }
}
