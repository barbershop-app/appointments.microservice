using AutoMapper;
using microservice.Infrastructure.Entities.DB;
using microservice.Web.API.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace microservice.Web.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        readonly IMapper _mapper;
        readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(IMapper mapper, ILogger<AppointmentsController> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

  
        //[HttpPost]
        //[Route("Create")]
        //public IActionResult Create([FromBody] UserDTOs.Create dto)
        //{
        //    try
        //    {
        //        var user =  _mapper.Map<Appointment>(dto);
        //        return Ok(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("The create function threw this exception : ", ex.ToString());
        //        return BadRequest("Failed to create user");
        //    }

        //}
    }
}
