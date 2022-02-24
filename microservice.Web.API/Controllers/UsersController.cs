using AutoMapper;
using microservice.Infrastructure.Entities.DB;
using microservice.Infrastructure.Entities.DTOs;
using microservice.Web.API.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace microservice.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IMapper _mapper;
        readonly ILogger<UsersController> _logger;

        public UsersController(IMapper mapper, ILogger<UsersController> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [Route("IsLoggedIn")]
        public IActionResult IsLoggedIn()
        {
            return Ok();
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] UserDTOs.Create dto)
        {
            try
            {
                var user =  _mapper.Map<User>(dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError("The create function threw this exception : ", ex.ToString());
                return BadRequest("Failed to create user");
            }

        }
    }
}
