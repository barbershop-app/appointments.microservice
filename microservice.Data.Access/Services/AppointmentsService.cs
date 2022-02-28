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

        public bool Create(Appointment appointment)
        {
            _unitOfWork.Appointments.Add(appointment);
            return _unitOfWork.Commit() > 0;
        }
        public bool Update(Appointment oldAppointment, Appointment appointment)
        {
            oldAppointment.HasBeenHandeled = appointment.HasBeenHandeled;
            _unitOfWork.Appointments.Update(oldAppointment);
            return _unitOfWork.Commit() > 0;
        }

        public bool Delete(Appointment appointment)
        {
            _unitOfWork.Appointments.Remove(appointment);
            return _unitOfWork.Commit() > 0;
        }
    }
}
