using E_Commerce_Application_API.DTOs;
using E_Commerce_Application_API.Interfaces;
using E_Commerce_Application_API.Models;
using E_Commerce_Application_API.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PokemonReviewApp.DevTools;
using System.Security.Claims;

namespace E_Commerce_Application_API.Controllers
{
    /// <summary>
    /// Auth route of the app. Authenticates users and returns JWT tokens or other authentication data.
    /// </summary>
    [Route("/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IMemoryCache Cache;
        private readonly JwtService JwtService;
        private readonly IUserRepository UserRepository;

        public AuthController(IMemoryCache cache, JwtService jwtService, IUserRepository userRepository)
        {
            this.Cache = cache;
            this.JwtService = jwtService;
            this.UserRepository = userRepository;
        }
        /// <summary>
        /// Logins with the specified credentials.
        /// </summary>
        /// <param name="credentials">The credentials of the user to be authenticated.</param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(AuthResponseDTO))]
        [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(401, Type = typeof(string))]
        public async Task<IActionResult> Login([FromBody] LoginDTO credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await UserRepository.GetUser(credentials.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password))
            {
                return Unauthorized("Invalid Credentials");
            }

            // Check if a valid token already exists in the cache for this user
            if (Cache.TryGetValue($"UserToken_{user.Id}", out string existingToken))
            {
                // Return the existing valid token
                return Ok(new { Token = existingToken });
            }

            var role = user.Email == "admin@gmail.com" ? "Admin" : "User";


            // If no valid token exists, generate a new JWT token
            var newToken = JwtService.GenerateJwtToken(user.Id, user.Email, role);

            // Store the token in the cache with expiration matching the token's expiration
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) // Set token expiry
            };

            Cache.Set($"UserToken_{user.Id}", newToken, cacheEntryOptions);

            return Ok(new AuthResponseDTO
            {
                Token = newToken,
                Role = role
            });
        }


        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult Logout()
        {
            try
            {
                int userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                // Remove the user's token from the cache
                Cache.Remove($"UserToken_{userId}");

                return NoContent();
            }
            catch (Exception e)
            {
                return RequestHandlerTool.HandleInternalServerError(e, "POST", "/auth/logout");
            }
        }
    }
}

