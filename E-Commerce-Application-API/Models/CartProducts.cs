using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application_API.Models
{
    public class CartProducts
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        // The qantity of the product in the shopping cart of this product
        [Range(0, 100)]
        public int Quantity { get; set; }
        // The shopping cart associated with this product
        public ShoppingCart ShoppingCart { get; set; }
        // The product associated with this shopping cart
        public Product Product { get; set; }
    }
}
