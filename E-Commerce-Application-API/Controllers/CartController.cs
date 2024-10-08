using E_Commerce_Application_API.DTOs;
using E_Commerce_Application_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DevTools;
using System.Security.Claims;

namespace E_Commerce_Application_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        /// <summary>
        /// The cart repository
        /// Used for retrieving and manipulating user's shopping carts.
        /// Depends on Dependency Injection
        /// </summary>
        private readonly ICartRepository CartRepository;
        private readonly IProductRepository ProductRepository;

        /// <param name="cartRepository">The cart repository dependency injection.</param>
        public CartController(ICartRepository cartRepository, IProductRepository productRepository)
        {
            this.CartRepository = cartRepository;
            this.ProductRepository = productRepository;
        }

        /**
         * 
         */
        /// <summary>
        /// Function to get users cart data.
        /// Does not need parameters since we have the users id from the JWT token.
        /// </summary>
        /// <returns>The users shopping cart data through DTO</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CartItemDTO>))]
        [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public async Task<IActionResult> GetUserCart()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = 0;
            try
            {
                // Get the authorised user id from the JWT token
                userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            catch (Exception e)
            {
                // Return internal server error on any non checked exception
                return RequestHandlerTool.HandleInternalServerError(e, "GET", "/api/Cart", "Error retrieving the user's id from authorisation");
            }
            // Retrieve the user's cart data
            try
            {
                return Ok(await CartRepository.GetUserCartData(userId));
            }

            catch (InvalidDataException ide)
            {
                return NotFound(ide.Message);
            }
            catch (Exception e)
            {
                // Return internal server error on any non checked exception
                return RequestHandlerTool.HandleInternalServerError(e, "GET", "/api/Cart");
            }

        }

        /// <summary>Adds to cart.</summary>
        /// <param name="product">The product to be added to the users cart.</param>
        /// <returns>
        ///   No content if successful addition. Else, returns error message and code
        /// </returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(500, Type = typeof(string))]
        public async Task<IActionResult> AddToCart([FromBody] CartSaveDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the productId is a valid one
            if (!(await ProductRepository.ProductExists(product.Id)))
            {
                ModelState.AddModelError("", "Wrong product id found");
                return BadRequest(ModelState);
            }

            int userId = 0;
            try
            {
                // Get the authorised user id from the JWT token
                userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            catch (Exception e)
            {
                // Return internal server error on any non checked exception
                return RequestHandlerTool.HandleInternalServerError(e, "POST", "/api/Cart", "Error retrieving the user's id from authorisation");
            }

            int cartId = 0;
            try
            {
                // Get the authorised user id from the JWT token
                cartId = await CartRepository.GetUsersCartId(userId);
            }
            catch (Exception e)
            {
                // Return internal server error on any non checked exception
                return RequestHandlerTool.HandleInternalServerError(e, "POST", "/api/Cart", "Error retrieving the user's cart id database");
            }

            // Try to add the product to the cart
            if (!(await CartRepository.AddProductToCart(cartId, product.Id)))
            {
                return RequestHandlerTool.HandleInternalServerError(new Exception("Error while saving product to cart"), "POST", "/api/Cart", "Product was with id = " + product.Id);
            }

            return Created();
        }


        /// <summary>Adds to cart.</summary>
        /// <param name="product">The product to be added to the users cart.</param>
        /// <returns>
        ///   No content if successful addition. Else, returns error message and code
        /// </returns>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(500, Type = typeof(string))]
        public async Task<IActionResult> ModifyCartItem([FromBody] MCartItemDTO cartItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = 0;
            try
            {
                // Get the authorised user id from the JWT token
                userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            catch (Exception e)
            {
                // Return internal server error on any non checked exception
                return RequestHandlerTool.HandleInternalServerError(e, "PUT", "/api/Cart", "Error retrieving the user's id from authorisation");
            }

            int cartId = 0;
            try
            {
                // Get the authorised user id from the JWT token
                cartId = await CartRepository.GetUsersCartId(userId);
            }
            catch (Exception e)
            {
                // Return internal server error on any non checked exception
                return RequestHandlerTool.HandleInternalServerError(e, "PUT", "/api/Cart", "Error retrieving the user's cart id from the database");
            }

            try
            {
                if (!await (CartRepository.ModifyCartItem(cartId, cartItem)))
                {
                    return RequestHandlerTool.HandleInternalServerError(new Exception("Error modifying cart item data"), "PUT", "/api/Cart");
                }
            }
            catch (Exception e)
            {
                // Return internal server error on any non checked exception
                return RequestHandlerTool.HandleInternalServerError(e, "PUT", "/api/Cart", "Error modifying carts database data");
            }


            return NoContent();
        }
    }
}