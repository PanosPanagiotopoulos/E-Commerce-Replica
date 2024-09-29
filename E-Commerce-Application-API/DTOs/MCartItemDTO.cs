using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application_API.DTOs
{
    /// <summary>
    /// Represents the input data needed for modification
    /// of a cart items data.
    /// </summary>
    public class MCartItemDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier of the product.
        /// </value>
        [Required(ErrorMessage = "Product id is required")]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity of the product in cart.
        /// </value>
        [Required(ErrorMessage = "Modification is required")]
        [Range(0, 100, ErrorMessage = "Quantity must be between 0 - 100")]
        public int Quantity { get; set; }
    }
}
