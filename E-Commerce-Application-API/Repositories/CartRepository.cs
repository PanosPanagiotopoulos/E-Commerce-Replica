using E_Commerce_Application_API.Data;
using E_Commerce_Application_API.DTOs;
using E_Commerce_Application_API.Interfaces;
using E_Commerce_Application_API.Mappers;
using E_Commerce_Application_API.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Application_API.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="E_Commerce_Application_API.Interfaces.ICartRepository" />
    public class CartRepository : ICartRepository
    {
        /// <summary>
        /// The context of the database operations.
        /// Used with Dependency Injection
        /// </summary>
        private readonly DataContext Context;
        /// <summary>
        /// The mapper instance. Use this property to map DTOs and entities.
        /// Used with Dependency Injection
        /// </summary>
        private readonly ICustomMapper CMapper;
        /// <summary>
        /// The product repository
        /// Used with Dependency Injection
        /// </summary>
        private readonly IProductRepository ProductRepository;

        public CartRepository(DataContext context, ICustomMapper mapper, IProductRepository productRepository)
        {
            this.Context = context;
            this.CMapper = mapper;
            this.ProductRepository = productRepository;
        }
        /// <summary>
        /// Gets the a particular users shopping cart.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The shopping cart instance for this particular user</returns>
        public async Task<ShoppingCart> GetUserShoppingCart(int userId)
        {
            return await Context.ShoppingCarts.Where(cart => cart.UserId == userId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the user cart data.
        /// Retrieves all the data needed for the Shopping Cart of a user 
        /// to put at the DTO
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Shopping cart not found for the user</exception>
        public async Task<IEnumerable<CartItemDTO>> GetUserCartData(int userId)
        {
            // Get the user's shopping cart
            int userCartId = await GetUsersCartId(userId);

            var cartProducts = await Context.CartProducts
            .Where(cp => cp.CartId == userCartId)
            .Include(cp => cp.Product)
            .ThenInclude(p => p.Images)
            .Include(cp => cp.Product)
            .ThenInclude(p => p.PaymentMethods)
            .ThenInclude(pm => pm.PaymentMethod)
            .Select(cp => new CartItemDTO
            {
                Product = CMapper.MapProductToProductDTO(cp.Product),
                Quantity = cp.Quantity
            })
            .AsNoTracking() // Add AsNoTracking for better read-only performance
            .ToListAsync();

            if (cartProducts == null)
            {
                throw new InvalidDataException("Null reference for cart products found");
            }

            return cartProducts;
        }

        public async Task<bool> AddProductToCart(int cartId, int productId)
        {
            // Check if the product already exists in the cart
            var cartProduct = Context.CartProducts.Where(cp => cp.CartId == cartId).Where(cp => cp.ProductId == productId).FirstOrDefault();

            // If the product does exist, up its quantity
            if (cartProduct != null)
            {
                // Up Quantity to the cart
                cartProduct.Quantity++;
                Context.Update(cartProduct);
                return (await Context.SaveChangesAsync()) > 0;
            }

            Context.CartProducts.Add(new CartProducts
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = 1
            });

            return (await Context.SaveChangesAsync()) > 0;
        }

        public async Task<int> GetUsersCartId(int userId)
        {
            int cartId = await Context.ShoppingCarts
                                      .Where(cart => cart.UserId == userId)
                                      .Select(cart => cart.Id)
                                      .FirstOrDefaultAsync();
            if (cartId == 0)
            {
                throw new Exception("Cart not found for user cart with id = " + userId);
            }

            return cartId;
        }

        public async Task<bool> ModifyCartItem(int cartId, MCartItemDTO cartItem)
        {
            // Get the instance of the cart item to be modified
            CartProducts toModifyCartItem = await Context.CartProducts.Where(cp => cp.CartId == cartId).Where(cp => cp.ProductId == cartItem.Id).FirstOrDefaultAsync();

            // Check for any server side errors of not finding the instance of checked values
            if (toModifyCartItem == null)
            {
                throw new Exception("Cart item not found in the cart with id = " + cartId + " and product id = " + cartItem.Id);
            }

            // If the change means that there is no more quantity left. Delete the cart-product record
            if (cartItem.Quantity == 0)
            {
                Context.Remove(toModifyCartItem);
                return (await Context.SaveChangesAsync()) > 0;
            }

            toModifyCartItem.Quantity = cartItem.Quantity;

            // Update quantity changes of the record in the database
            Context.Update(toModifyCartItem);

            return (await Context.SaveChangesAsync()) > 0;
        }
    }
}
