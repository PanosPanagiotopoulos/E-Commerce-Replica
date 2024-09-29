using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application_API.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Pid { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(40, MinimumLength = 10, ErrorMessage = "The field must be between 10 and 40 characters long.")]

        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "The field must be between 10 and 100 characters long.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.1, 100000, ErrorMessage = "The price must be between 0.1 and 100000.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The field must be between 3 and 20 characters long.")]
        public string Category { get; set; }
        [Required(ErrorMessage = "ShippingCost is required.")]
        [Range(0.1, 100, ErrorMessage = "Shipping cost must be between 0.1 and 100.")]
        public decimal ShippingCost { get; set; }
        // The image urls of the product returned
        public string[] ImageURLS { get; set; }
        // The possible payment methods of the product returned
        public string[] PaymentMethods { get; set; }
    }
}
