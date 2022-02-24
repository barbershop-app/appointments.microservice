using microservice.Infrastructure.Entities.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microservice.Core.IServices
{
    public interface IUsersService
    {
        public bool Create(User user);
        public bool Update(User user);
        public bool Delete(Guid id);
    }
}
