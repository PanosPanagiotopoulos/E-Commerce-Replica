using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application_API.DTOs
{
    public class AuthResponseDTO
    {
        [Required(ErrorMessage = "Token value is required")]
        public string Token { get; set; }
        [Required(ErrorMessage = "Role value is required")]
        public string Role { get; set; }
    }
}
