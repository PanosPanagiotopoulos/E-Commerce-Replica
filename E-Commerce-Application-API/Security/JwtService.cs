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
        public string GenerateJwtToken(int userId)
        {
            // Create JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = Configuration["Jwt:Issuer"],
                Audience = Configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

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
