using AutoMapper;
using microservice.Core.IServices;
using microservice.Infrastructure.Entities.DB;
using microservice.Infrastructure.Entities.DTOs;
using microservice.Infrastructure.Utils;
using microservice.Web.API.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace microservice.Web.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        readonly IMapper _mapper;
        readonly ILogger<AppointmentsController> _logger;
        readonly IAppointmentService _appointmentService;
        readonly IHttpClientFactory _httpClientFactory;
        readonly IConfiguration _configuration;

        public AppointmentsController(IMapper mapper, ILogger<AppointmentsController> logger, IAppointmentService appointmentService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _mapper = mapper;
            _logger = logger;
            _appointmentService = appointmentService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] AppointmentDTOs.Create dto)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("localhost");

                #region Check if the user is active

                var response = await httpClient.GetAsync($"{_configuration.GetValue<string>(Constants.USERS_MICROSERVICE_API)}/Users/IsActive/{dto.UserId}");


                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(@$"'{Constants.USERS_MICROSERVICE_API}/Users/IsActive' responded with, {{StatusCode :{response.StatusCode}}}");

                #endregion



                #region Get the day booking limit
                var values = new
                {
                    BarberShopId = dto.BarberShopId,
                    Day = dto.Date.DayOfWeek
                };


                var json = JsonConvert.SerializeObject(values);
                var content = new StringContent(json, Encoding.UTF8, "application/json");



                 response = await httpClient.PostAsync($"{_configuration.GetValue<string>(Constants.CONFIGURATION_MICROSERVICE_API)}/Management/GetBookingDayLimit", content);

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(@$"'{Constants.CONFIGURATION_MICROSERVICE_API}/Management/GetBookingDayLimit' responded with, {{StatusCode :{response.StatusCode}}}");
                



                var bookingLimit = int.Parse(await response.Content.ReadAsStringAsync());

                #endregion



                #region Check if there are avialable spots to book and if the user already has a registered booking on this day

                var appointments = _appointmentService.GetAllAsQueryable().Where(x => x.Date.ToShortDateString() == dto.Date.ToShortDateString());

                if (appointments.Any(x => x.UserId == dto.UserId))
                    return BadRequest("This user already has a registered booking on this day.");

                if (appointments.Count() >= bookingLimit)
                    return BadRequest("There are no available spots for booking on this day.");

                #endregion




                var appointment = _mapper.Map<Appointment>(dto);

                var res = _appointmentService.Create(appointment);


                if (res)
                    return Ok("Appointment has been booked.");



                return BadRequest("Appointment has not been booked.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest("Something went wrong.");
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
                    return BadRequest("Appointment does not exist.");

                var appointment = _mapper.Map<Appointment>(dto);


                var res = _appointmentService.Update(oldAppointment, appointment);


                if (res)
                    return Ok("Appointment has been updated.");



                return BadRequest("Appointment has not been updated.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest("Something went wrong.");
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
                    return BadRequest("Appointment does not exist.");

               

                var res = _appointmentService.Delete(appointment, true);


                if (res)
                    return Ok("Appointment has been deleted.");



                return BadRequest("Appointment has not been deleted.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest("Something went wrong.");

            }

        }
    }
}
