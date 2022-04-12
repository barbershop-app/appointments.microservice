using AutoMapper;
using microservice.Core.IServices;
using microservice.Infrastructure.Entities.DB;
using microservice.Infrastructure.Entities.DTOs;
using microservice.Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace microservice.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        readonly IMapper _mapper;
        readonly ILogger<AppointmentsController> _logger;
        readonly IAppointmentService _appointmentService;
        readonly IHttpClientService _httpClientFactoryService;

        public AppointmentsController(IMapper mapper, ILogger<AppointmentsController> logger, IAppointmentService appointmentService, IHttpClientService httpClientFactoryService)
        {
            _mapper = mapper;
            _logger = logger;
            _appointmentService = appointmentService;
            _httpClientFactoryService = httpClientFactoryService;
        }




        [HttpGet]
        [Route("GetBookedAppointment/{Id}")]
        public IActionResult GetBookedAppointments(Guid Id)
        {
            try
            {
                var appointment = _appointmentService.GetAllAsQueryable(false).Where(x => x.UserId == Id && x.Date > DateTime.Today).FirstOrDefault();

                if (appointment == null)
                    return BadRequest(new { message = "User has no booked appointments." });



                var todayAppointments = _appointmentService.GetAllAsQueryable(false).Where(x => x.Date.ToShortDateString() == DateTime.Today.ToShortDateString() && !x.HasBeenHandeled);


                int numberInQueue = todayAppointments.Where(x => x.Date < appointment.Date).Count();


         
                    return Ok(new {
                        Id = appointment.Id,
                        Date = appointment.Date,
                        HasBeenHandeled = appointment.HasBeenHandeled,
                        numberInQueue = numberInQueue,
                    }); 


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new { message = "Something went wrong." });
            }
        }


        [HttpGet]
        [Route("GetTodayAppointments")]
        public IActionResult GetTodayAppointments()
        {
            try
            {
                var appointments = _appointmentService.GetAllAsQueryable(false).Where(x => x.Date.ToShortDateString() == DateTime.Today.ToShortDateString());


                var filteredAppointments = new List<object>();

                foreach (var appointment in appointments)
                {
                    filteredAppointments.Add(new
                    {
                        Id = appointment.Id,
                        Date = appointment.Date,
                        HasBeenHandeled = appointment.HasBeenHandeled
                    });
                }


                if (appointments.Count() != 0)
                    return Ok(new
                    {
                        bookedAppointments = filteredAppointments
                    });

                return BadRequest(new { message = "There are no appointments for today." });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new { message = "Something went wrong." });
            }
        }


        [HttpPost]
        [Route("AvailableSlots")]
        public async Task<IActionResult> AvailableSlots([FromBody] AppointmentDTOs.AvailableSlots dto)
        {
            try
            {
                if (DateTime.Today > dto.Date)
                    return BadRequest(new { message = "Invalid Date." });


                var bookingLimit = await _httpClientFactoryService.GetBookingDayLimit(dto.BarberShopId, dto.Date.DayOfWeek);


                var appointments = _appointmentService.GetAllAsQueryable(false).Where(x => x.Date.ToShortDateString() == dto.Date.ToShortDateString());

                if (appointments.Count() <= bookingLimit)
                    return Ok(new { availableSlots = bookingLimit - appointments.Count() });

                return BadRequest(new { message = "There are no avialable Slots on this date!" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new { message = "Something went wrong." });
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] AppointmentDTOs.Create dto)
        {
            try
            {
                if (DateTime.Today > dto.Date)
                    return BadRequest(new { message = "Invalid Date." });


                await _httpClientFactoryService.CheckIfUserIsActive(dto.UserId);


                var bookingLimit = await _httpClientFactoryService.GetBookingDayLimit(dto.BarberShopId, dto.Date.DayOfWeek);


                var appointments = _appointmentService.GetAllAsQueryable(false).Where(x => x.Date.ToShortDateString() == dto.Date.ToShortDateString());

                if (appointments.Count() >= bookingLimit)
                    return BadRequest(new { message = "There are no available spots for booking on this day." });


                var bookedAppointment = _appointmentService.GetAllAsQueryable(false).Where(x => x.UserId == dto.UserId && x.Date > DateTime.Today).FirstOrDefault();

                if (bookedAppointment != null)
                    return BadRequest(new {message = "You already have an appointment booked on this day."});



                var appointment = _mapper.Map<Appointment>(dto);

                var res = _appointmentService.Create(appointment);


                if (res)
                    return Ok(new { message = "Appointment has been booked." });
  



                return BadRequest(new { message = "Appointment has not been booked." });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new { message = "Something went wrong." });
            }

        }
        

        [HttpPost]
        [Route("Update")]
        public IActionResult Update([FromBody] AppointmentDTOs.Update dto)
        {
            try
            {
                var oldAppointment = _appointmentService.GetById(dto.Id);

                if (oldAppointment == null)
                    return BadRequest(new { message = "Appointment does not exist." });

                var appointment = _mapper.Map<Appointment>(dto);


                var res = _appointmentService.Update(oldAppointment, appointment);


                if (res)
                    return Ok(new { message = "Appointment has been updated." });



                return BadRequest(new { message = "Appointment has not been updated." });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new { message = "Something went wrong." });
            }

        }

        [HttpPost]
        [Route("Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var appointment = _appointmentService.GetById(id);

                if (appointment == null)
                    return BadRequest(new { message = "Appointment does not exist." });

               

                var res = _appointmentService.Delete(appointment, true);


                if (res)
                    return Ok(new { message = "Appointment has been deleted." });



                return BadRequest(new { message = "Appointment has not been deleted." });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new { message = "Something went wrong." });

            }

        }
    }
}
