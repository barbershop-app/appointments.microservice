using AutoMapper;
using microservice.Infrastructure.Entities.DB;
using microservice.Infrastructure.Entities.DTOs;

namespace microservice.Web.API.Internal
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<AppointmentDTOs.Create, Appointment>();
        }
    }
}
