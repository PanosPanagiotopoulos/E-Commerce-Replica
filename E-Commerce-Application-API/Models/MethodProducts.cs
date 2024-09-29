namespace E_Commerce_Application_API.Models
{
    public class MethodProducts
    {
        /**
         * This is a join table for the many to many relationship of 
         * products and payment methods. For every product there are many possible unique pauyment methods
         */
        public int MethodId { get; set; }
        public int ProductId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Product Product { get; set; }
    }
}
