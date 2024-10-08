using AutoMapper;
using E_Commerce_Application_API.DTOs;
using E_Commerce_Application_API.Interfaces;
using E_Commerce_Application_API.Security;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DevTools;

namespace E_Commerce_Application_API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository UserRepository;
        private readonly IMapper Mapper;
        private readonly JwtService JwtService;

        public UserController(JwtService jwtService, IUserRepository userRepository, IMapper mapper)
        {
            this.JwtService = jwtService;
            this.UserRepository = userRepository;
            this.Mapper = mapper;
        }

        [Authorise]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(UserDTO))]
        [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500, Type = typeof(string))]
        public async Task<IActionResult> GetUserData([FromQuery] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await UserRepository.GetUser(userId);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<UserDTO>(user));
            }
            catch (Exception e)
            {
                return RequestHandlerTool.HandleInternalServerError(e, "GET", "/api/User");
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(500, Type = typeof(string))]
        public async Task<IActionResult> CreateUser([FromBody] RUserDTO createUserData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if ((await UserRepository.UserExists(createUserData.Email)))
                {
                    ModelState.AddModelError("", "User with this email already exists.");
                    return BadRequest(ModelState);
                }

                if (!(await UserRepository.CreateUser(createUserData)))
                {
                    return RequestHandlerTool.HandleInternalServerError(new Exception("Error while saving a new user"), "POST", "/api/User");
                }

                return Created();
            }

            catch (ArgumentException ae)
            {
                ModelState.AddModelError("", ae.Message);
                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                return RequestHandlerTool.HandleInternalServerError(e, "POST", "/api/User");
            }
        }
    }
}
