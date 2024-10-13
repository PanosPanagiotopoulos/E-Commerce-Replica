using E_Commerce_Application_API.Data;
using E_Commerce_Application_API.DTOs;
using E_Commerce_Application_API.Interfaces;
using E_Commerce_Application_API.Mappers;
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
        private readonly ICustomMapper Mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="context">The context of the database connection.</param>
        public ProductRepository(DataContext context, ICustomMapper mapper)
        {
            this.Context = context;
            this.Mapper = mapper;
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
            return await Context.Products
                    .AsNoTracking()
                    .OrderBy(product => product.Id)
                    .ToListAsync();
        }

        public async Task<PagedProductsResDTO> GetProductsPaged(
        int pageNumber = 1,
        int pageSize = 10,
        FiltersDTO? filters = null)
        {
            // Calculate where the pagination will take place
            int skip = (pageNumber - 1) * pageSize;

            // Query the products with pagination and sorting
            var query = Context.Products
            .AsNoTracking() // Improves read-only performance
            .Include(p => p.Images)
            .Include(p => p.PaymentMethods)
            .ThenInclude(mp => mp.PaymentMethod)
            .AsQueryable();

            if (filters == null)
            {
                query = query.OrderBy(p => p.Id); // Default sorting by Id
            }

            // Apply sorting
            if (filters != null)
            {
                // Define valid sort columns with correct case
                var validSortByColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Id", "Id" },
                    { "Title", "Title" },
                    { "Price", "Price" },
                    { "Category", "Category" }
                };

                // Validate and map SortBy
                if (string.IsNullOrEmpty(filters.SortBy) || !validSortByColumns.TryGetValue(filters.SortBy, out var sortByColumn))
                {
                    throw new ArgumentException($"Invalid SortBy value. Must be one of: {string.Join(", ", validSortByColumns.Keys)}");
                }

                // Validate SortDirection
                if (string.IsNullOrEmpty(filters.SortDirection) ||
                    !(filters.SortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
                      filters.SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("Invalid SortDirection value. Must be 'asc' or 'desc'");
                }

                query = filters.SortDirection!.ToLower() == "desc"
                    ? query.OrderByDescending(p => EF.Property<object>(p, sortByColumn))
                    : query.OrderBy(p => EF.Property<object>(p, sortByColumn));
            }

            // Get total product count
            int totalProducts = await query.CountAsync();

            // Apply pagination
            var products = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();


            // Calculate total pages
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            int remainingPages = totalPages >= pageNumber ? totalPages - pageNumber : 0;

            // Return the result with pagination info
            return new PagedProductsResDTO
            {
                Products = products.Select(product => Mapper.MapProductToProductDTO(product)),
                PagesRemaining = remainingPages,
                CurrentPage = pageNumber,
                HasMorePages = remainingPages > 0
            };
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

        public async Task<bool> AddNewProduct(ProductDTO product)
        {


            return true;
        }
    }
}
