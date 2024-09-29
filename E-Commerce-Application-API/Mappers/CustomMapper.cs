using E_Commerce_Application_API.DTOs;
using E_Commerce_Application_API.Models;

namespace E_Commerce_Application_API.Mappers
{
    public class CustomMapper : ICustomMapper
    {
        //
        /// <summary> 
        /// Map Product data to ready to transfer product data
        /// Needed mapping for images is done to the URLs 
        /// Needed mapping for payment methods we just get the array of method property.
        /// </summary>
        /// <param name="product">The product instance to be mapped.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public ProductDTO MapProductToProductDTO(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Pid = product.Pid,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                ShippingCost = product.ShippingCost,
                ImageURLS = product.Images?.Select(img => img.BlobURL).ToArray(),
                PaymentMethods = product.PaymentMethods?
                    .Select(mp => mp.PaymentMethod.method)
                    .ToArray()
            };
        }
    }
}
