using E_Commerce_Application_API.Data;
using E_Commerce_Application_API.Interfaces;
using E_Commerce_Application_API.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Application_API.Repositories
{
    /// <summary>
    /// Represents a repository for managing products.
    /// </summary>
    /// <seealso cref="E_Commerce_Application_API.Interfaces.IProductRepository" />
    public class ProductRepository : IProductRepository
    {
        /// <summary>
        /// The context for database operations.  
        /// Use this property to interact with the database. 
        /// Do not create a new instance of DbContext in this class.
        /// </summary>
        private readonly DataContext Context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="context">The context of the database connection.</param>
        public ProductRepository(DataContext context)
        {
            this.Context = context;
        }
        public async Task<Product> GetProductById(int id)
        {
            return await Context.Products.Where(product => product.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductById(string pid)
        {
            return await Context.Products.Where(product => product.Pid == pid).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await Context.Products.OrderBy(product => product.Id).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByIdList(IEnumerable<int> ids)
        {
            /*
             * Query for products using Include method to include related entities
             * that are needed to create the DTOs for the controllers
             */
            return await Context.Products.Include(product => product.Images)
                                         .Include(product => product.PaymentMethods)
                                         .ThenInclude(mp => mp.PaymentMethod)
                                         .Where(product => ids.Contains(product.Id)).ToListAsync();
        }

        public async Task<bool> ProductExists(int id)
        {
            return await Context.Products.AnyAsync(product => product.Id == id);
        }
    }
}
