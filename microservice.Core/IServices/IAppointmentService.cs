using microservice.Infrastructure.Entities.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microservice.Core.IServices
{
    public interface IAppointmentService
    {
        public bool Create(Appointment appointment);
        public bool Update(Appointment oldAppointment, Appointment appointment);
        public bool Delete(Appointment appointment, bool commit);
        public Appointment GetById(Guid Id);
        public IEnumerable<Appointment> GetAllAsQueryable();
    }
}
