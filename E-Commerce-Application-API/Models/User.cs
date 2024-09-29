using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application_API.Models
{
    public class User
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
        [RegularExpression("^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*(),.?\":{}|<>])[A-Za-z0-9!@#$%^&*(),.?\":{}|<>]{6,}$",
            ErrorMessage = "Password must be at least 6 characters long, contain at least one uppercase letter, one number, and one special character.")]
        public string Password { get; set; }
        public DateTime RegisteredDate { get; set; }
        // The users unique and only shopping cart
        public ShoppingCart ShoppingCart { get; set; }

        public User()
        {
            this.RegisteredDate = DateTime.UtcNow;
        }
    }
}
