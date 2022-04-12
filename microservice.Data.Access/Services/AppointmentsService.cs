using microservice.Core;
using microservice.Core.IServices;
using microservice.Infrastructure.Entities.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microservice.Data.Access.Services
{
    public class AppointmentsService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AppointmentsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Appointment GetById(Guid Id)
        {
            return _unitOfWork.Appointments.GetByIdIncluded(Id);
        }


        public IEnumerable<Appointment> GetAllAsQueryable(bool track)
        {
            if (track)
                return _unitOfWork.Appointments.GetAllAsQueryable();

            return _unitOfWork.Appointments.GetAllAsQueryableAsNoTracking();

        }


        public bool Create(Appointment appointment)
        {

            var newDate = new DateTime(appointment.Date.Year, appointment.Date.Month, appointment.Date.Day,DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            appointment.Date = newDate;

            _unitOfWork.Appointments.Add(appointment);
            return _unitOfWork.Commit() > 0;
        }
        public bool Update(Appointment oldAppointment, Appointment appointment)
        {
            oldAppointment.HasBeenHandeled = appointment.HasBeenHandeled;
            _unitOfWork.Appointments.Update(oldAppointment);
            return _unitOfWork.Commit() > 0;
        }

        public bool Delete(Appointment appointment, bool commit)
        {
            _unitOfWork.Appointments.Remove(appointment);

            if (commit)
            return _unitOfWork.Commit() > 0;

            return true;
        }
    }
}
