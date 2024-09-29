using E_Commerce_Application_API.DTOs;
using E_Commerce_Application_API.Models;

namespace E_Commerce_Application_API.Interfaces
{
    /// <summary>Repository pattern for database actions
    /// Of the cart model and its related models</summary>
    public interface ICartRepository
    {

        /// <summary>Gets the user cart data.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        ///   Method to retrieve the user's cart items from the database
        /// </returns>
        Task<IEnumerable<CartItemDTO>> GetUserCartData(int userId);

        /// <summary>Gets the user shopping cart.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        ///   Method to get a particular users ShoppingCart from the database
        /// </returns>
        Task<ShoppingCart> GetUserShoppingCart(int userId);

        /// <summary>Gets the users cart identifier.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        ///   the users cart identifier
        /// </returns>
        Task<int> GetUsersCartId(int userId);
        /// <summary>Adds the product to cart-product relationship table.</summary>
        /// <param name="cartId">The cart identifier where the product will be added.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns>
        ///   Returns true if the product was successfully added to the cart, false otherwise.
        /// </returns>
        Task<bool> AddProductToCart(int cartId, int productId);
        Task<bool> ModifyCartItem(int cartId, MCartItemDTO cartItem);
    }
}
