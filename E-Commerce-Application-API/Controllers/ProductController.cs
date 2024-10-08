
using E_Commerce_Application_API.DTOs;
using E_Commerce_Application_API.Interfaces;
using E_Commerce_Application_API.Mappers;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DevTools;

namespace E_Commerce_Application_API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductRepository ProductRepository;
        private readonly ICustomMapper Mapper;

        public ProductController(IProductRepository productRepository, ICustomMapper mapper)
        {
            this.ProductRepository = productRepository;
            this.Mapper = mapper;
        }

        [HttpGet("/id")]
        [ProducesResponseType(200, Type = typeof(ProductDTO))]
        [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(ProductDTO))]
        public async Task<IActionResult> GetProduct([FromQuery] int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!(await ProductRepository.ProductExists(productId)))
            {
                return NotFound("Product with id = " + productId + " not found");
            }

            try
            {
                // Get the product from the database
                // Used the method to get many products from a range of ids to get the navigation properties as well
                // Then since we know it will return only 1 product , we use First() to get the first one
                // Then return the DTO version of it.
                return Ok(Mapper.MapProductToProductDTO((await ProductRepository.GetProductsByIdList([productId])).First()));
            }
            catch (Exception e)
            {
                return RequestHandlerTool.HandleInternalServerError(e, "GET", "/api/Product");
            }
        }

        [HttpGet("/products")]
        [ProducesResponseType(200, Type = typeof(PagedProductsResDTO))]
        [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(500, Type = typeof(string))]
        public async Task<IActionResult> GetProducts([FromQuery] FiltersDTO? filter = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // Check for request input mismtaches
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the filters object is non-null but has all null properties
            if (filter != null &&
                string.IsNullOrEmpty(filter.SortBy) &&
                string.IsNullOrEmpty(filter.SortDirection))
            {
                filter = null; // Explicitly set filters to null if all properties are null or empty
            }

            // Sanetize input
            if (page < 1 || pageSize < 1)
            {
                ModelState.AddModelError("", "Page and Pagesize must be greater or equal to 1");
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await ProductRepository.GetProductsPaged(page, pageSize, filter));
            }

            catch (ArgumentException e1)
            {
                ModelState.AddModelError("", e1.Message);
                return BadRequest(ModelState);
            }

            catch (Exception e2)
            {
                return RequestHandlerTool.HandleInternalServerError(e2, "GET", "/api/Product/products");
            }
        }
    }
}
