﻿using AutoMapper;
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



        [HttpPost]
        [Route("AvailableSlots")]
        public async Task<IActionResult> AvailableSlots([FromBody] AppointmentDTOs.AvailableSlots dto)
        {
            try
            {
                if (DateTime.Today > dto.Date)
                    return BadRequest("Invalid Date.");


                var bookingLimit = await _httpClientFactoryService.GetBookingDayLimit(dto.BarberShopId, dto.Date.DayOfWeek);


                var appointments = _appointmentService.GetAllAsQueryable(false).Where(x => x.Date.ToShortDateString() == dto.Date.ToShortDateString());

                if (appointments.Count() <= bookingLimit)
                    return Ok(bookingLimit - appointments.Count());

                return BadRequest("There are no avialable Slots on this date!");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest("Something went wrong.");
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] AppointmentDTOs.Create dto)
        {
            try
            {

                await _httpClientFactoryService.CheckIfUserIsActive(dto.UserId);


                var bookingLimit = await _httpClientFactoryService.GetBookingDayLimit(dto.BarberShopId, dto.Date.DayOfWeek);


                var appointments = _appointmentService.GetAllAsQueryable(false).Where(x => x.Date.ToShortDateString() == dto.Date.ToShortDateString());

                if (appointments.Any(x => x.UserId == dto.UserId))
                    return BadRequest("This user already has a registered booking on this day.");

                if (appointments.Count() >= bookingLimit)
                    return BadRequest("There are no available spots for booking on this day.");



                var appointment = _mapper.Map<Appointment>(dto);

                var res = _appointmentService.Create(appointment);


                if (res)
                {
                    _ = Task.Run(() =>
                      {

                          //Use SignalR to update the user on how much time left for his appointment

                      });



                    return Ok("Appointment has been booked.");
                }



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
