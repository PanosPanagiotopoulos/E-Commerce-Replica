using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application_API.DTOs
{

    /// <summary>
    /// DTO class to represent a cart item.
    /// Used to manipulate Cart-Products relationship table in database
    /// </summary>
    public class CartItemDTO
    {
        /// <summary>
        /// Gets or sets the product DTO.
        /// </summary>
        /// <value>
        /// The product in the given cart - product instance.
        /// </value>
        public ProductDTO Product { get; set; }
        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity of the product <> <>.
        /// </value>
        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, 100)]
        public int Quantity { get; set; }
    }
}
