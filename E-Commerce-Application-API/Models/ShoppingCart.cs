namespace E_Commerce_Application_API.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        // The user where this ShoppingCart is associated with
        public int UserId { get; set; }
        public User User { get; set; }
        // The associated products with this ShoppingCart
        public ICollection<CartProducts> CartProducts { get; set; }
    }
}
