using E_Commerce_Application_API.DTOs;
using E_Commerce_Application_API.Models;

namespace E_Commerce_Application_API.Interfaces
{
    /// <summary>
    /// Interfaces for product repository operations.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Retrieve the products from the database
        /// </summary>
        /// <returns>All the products in the database</returns>
        Task<IEnumerable<Product>> GetProducts();

        /// <summary>
        /// Retrieve a product from the database with the database id
        /// </summary>
        /// <param name="id">The identifier of the product.</param>
        /// <returns></returns>
        Task<Product> GetProductById(int id);
        // Retrieve a product from the database with the products custom string id (code)
        /// <summary>
        /// Gets the product by identifier.
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <returns></returns>
        Task<Product> GetProductById(string pid);
        // Retrieve products from the database with a list of ids
        /// <summary>
        /// Gets the products by identifier list.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetProductsByIdList(IEnumerable<int> ids);
        /// <summary>
        /// Products the exists.
        /// </summary>
        /// <param name="id">The product identifier.</param>
        /// <returns>True or false depending if it does or not</returns>
        Task<Boolean> ProductExists(int id);

        /// <summary>Gets the products paged.</summary>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sortBy">The sort parameter if requested.</param>
        /// <param name="sortDirection">The sort direction to combine with the sorting parameter.</param>
        /// <returns>
        ///   The DTO of the products reuqested paged
        ///   Some more pagination metadata for the client
        ///   Like how many pages are left and the current page number
        /// </returns>
        Task<PagedProductsResDTO> GetProductsPaged(
        int pageNumber = 1,
        int pageSize = 10,
        FiltersDTO? filters = null);

        /// <summary>Adds the new product.</summary>
        /// <param name="product">The new product to be saved.</param>
        /// <returns>
        ///   boolean depending on successful save
        /// </returns>
        Task<bool> AddNewProduct(ProductDTO product);
    }

}
