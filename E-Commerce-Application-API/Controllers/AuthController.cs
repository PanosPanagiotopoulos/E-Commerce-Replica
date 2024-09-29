using E_Commerce_Application_API.DTOs;
using E_Commerce_Application_API.Security;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Application_API.Controllers
{
    /// <summary>
    /// Auth route of the app. Authenticates users and returns JWT tokens or other authentication data.
    /// </summary>
    [Route("/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        /// <summary>
        /// The JWT service of the app
        /// </summary>
        private readonly JwtService JwtService;

        /// <param name="jwtService">The JWT service injection.</param>
        public AuthController(JwtService jwtService)
        {
            this.JwtService = jwtService;
        }
        /// <summary>
        /// Logins with the specified credentials.
        /// </summary>
        /// <param name="credentials">The credentials of the user to be authenticated.</param>
        /// <returns></returns>
        [HttpPost("/login")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> Login([FromBody] LoginDTO credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Authenticate the user and get user instance if succeed

            int userId = 2;
            return Ok(JwtService.GenerateJwtToken(userId));
        }
    }
}

