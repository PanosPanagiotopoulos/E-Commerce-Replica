using E_Commerce_Application_API.DTOs;
using E_Commerce_Application_API.Models;

namespace E_Commerce_Application_API.Mappers
{
    public interface ICustomMapper
    {
        /*
         * Custom mapping methods for DTOs and entities.
         */
        ProductDTO MapProductToProductDTO(Product product);
    }
}
