using microservice.Infrastructure.Entities.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microservice.Core.IRepositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        IEnumerable<Appointment> GetAllAsQueryable();
        IEnumerable<Appointment> GetAllAsQueryableAsNoTracking();
        Appointment GetByIdIncluded(Guid id);

    }
}
