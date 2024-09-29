using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application_API.DTOs
{
    /// <summary>
    /// DTO class to represent a cart input data.
    /// Used to save new cart item
    /// </summary>
    public class CartSaveDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier of the product.
        /// </value>
        [Required(ErrorMessage = "Product id is required")]
        public int Id { get; set; }
    }
}
