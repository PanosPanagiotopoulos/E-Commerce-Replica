using E_Commerce_Application_API.DTOs;
using E_Commerce_Application_API.Models;

namespace E_Commerce_Application_API.Interfaces
{
    /// <summary>
    /// User repository function that connect entities to database
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets the user by his unique id.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The particular users data</returns>
        Task<User> GetUser(int userId);
        /// <summary>
        /// Gets the user by username and password. Used for authentication.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>

        /// <returns> The particular users data </returns>
        Task<User> GetUser(string email);
        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="userDTO">The new users data.</param>
        /// <returns>boolean depending on success or failure</returns>
        Task<bool> CreateUser(RUserDTO user);
        /// <summary>
        /// Updates the user data.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="ArgumentException">Thrown when the mapping from dto fails and sends corresponding message.</exception>
        /// <exception cref="Exception">Thrown when database managing fails</exception>
        /// <returns>boolean depending on success or failure</returns>
        Task<bool> UpdateUser(RUserDTO user);
        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <exception cref="ArgumentException">Thrown when update user does not exist</exception>
        /// <returns>boolean depending on success or failure</returns>
        Task<bool> DeleteUser(int userId);
        /// <summary>
        /// Check if user exists based on his identifier
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <exception cref="ArgumentException">Thrown when the user with the specific id does not exist.</exception>
        /// <returns>boolean depending on success or failure</returns>
        Task<bool> UserExists(int userId);
        /// <summary>
        /// Check if a particular user exists using his email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>boolean depending on success or failure</returns>
        Task<bool> UserExists(string email);
    }
}
