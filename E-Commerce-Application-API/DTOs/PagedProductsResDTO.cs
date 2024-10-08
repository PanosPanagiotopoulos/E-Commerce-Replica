namespace E_Commerce_Application_API.DTOs
{
    public class PagedProductsResDTO
    {
        // The main payload which is the data of the products reuqested
        public IEnumerable<ProductDTO> Products { get; set; }
        // The remaining pages from the products table
        public int PagesRemaining { get; set; }
        // The current page returned
        public int CurrentPage { get; set; }
        public bool HasMorePages { get; set; }
    }
}
