using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace E_Commerce_Application_API.Security
{
    /// <summary>
    /// Singleton service instance to handle authentication
    /// </summary>
    public class JwtService
    {
        /// <summary>
        /// The configuration of the application
        /// </summary>
        private readonly IConfiguration Configuration;

        public JwtService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Generates the JWT token.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public string GenerateJwtToken(int userId, string email, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email)
            };
            claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Configuration["Jwt:Issuer"],
                audience: Configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        /// <summary>
        /// Generates the JWT secret key using random algorithm.
        /// </summary>
        /// <returns></returns>
        private string GenerateJwtSecretKey()
        {
            // Generate a 256-bit (32-byte) key
            byte[] key = new byte[32];
            RandomNumberGenerator.Fill(key);

            return Convert.ToBase64String(key);
        }
    }
}
