using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application_API.DTOs
{
    /// <summary>
    /// DTO class to represent a the login input data.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required(ErrorMessage = "Email  is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [RegularExpression("^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*(),.?\":{}|<>])[A-Za-z0-9!@#$%^&*(),.?\":{}|<>]{6,}$",
            ErrorMessage = "Password must be at least 6 characters long, contain at least one uppercase letter, one number, and one special character.")]
        public string Password { get; set; }
    }
}
