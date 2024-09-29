namespace E_Commerce_Application_API.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string method { get; set; }
        public ICollection<MethodProducts> MethodProducts { get; set; }
    }
}
