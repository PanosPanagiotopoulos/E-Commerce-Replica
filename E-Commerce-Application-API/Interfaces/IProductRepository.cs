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
    }

}
