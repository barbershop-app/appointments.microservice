using microservice.Core.IRepositories;
using microservice.Infrastructure.Entities.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microservice.Data.SQL.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        private AppointmentsContext? _context { get { return Context as AppointmentsContext; } }
        public AppointmentRepository(AppointmentsContext context) : base(context)
        {

        }
        public Appointment GetByIdIncluded(Guid id)
        {
            return _context.Appointments.Where(s => s.Id == id).FirstOrDefault();
        }

        public IEnumerable<Appointment> GetAllAsQueryable()
        {
            return _context.Appointments.AsQueryable();
        }


        public IEnumerable<Appointment> GetAllAsQueryableAsNoTracking()
        {
            return _context.Appointments.AsNoTracking().AsQueryable();

        }

    }
}
