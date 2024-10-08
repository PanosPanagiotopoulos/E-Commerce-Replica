using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application_API.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Firstname is required.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "The field must be between 2 and 25 characters long.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Lastname is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "The field must be between 2 and 30 characters long.")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Phonenumber  is required.")]
        [Phone(ErrorMessage = "Phonenumber is required.")]
        public string Phonenumber { get; set; }
        [Required(ErrorMessage = "Email  is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string Email { get; set; }
    }
}
