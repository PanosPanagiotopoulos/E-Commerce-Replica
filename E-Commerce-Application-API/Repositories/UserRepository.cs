using AutoMapper;
using E_Commerce_Application_API.Data;
using E_Commerce_Application_API.DTOs;
using E_Commerce_Application_API.Interfaces;
using E_Commerce_Application_API.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Application_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext Context;
        private readonly IMapper Mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            this.Context = context;
            this.Mapper = mapper;
        }

        public async Task<bool> CreateUser(RUserDTO user)
        {
            User newUser = Mapper.Map<User>(user);

            if (newUser == null)
            {
                // Handle the case where the mapping failed
                throw new ArgumentException("Null mapping from dto to model of user");
            }

            // Hash the user's password before saving it
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            Context.Users.Add(newUser);


            if (await Context.SaveChangesAsync() <= 0)
            {
                // Handle the case where the save operation failed
                throw new Exception("Failed to save user to the database");
            }


            // Generate a shopping cart for the user based on their 1-1 relationship
            ShoppingCart newUserCart = new ShoppingCart();
            newUserCart.User = newUser;
            newUserCart.CartProducts = new List<CartProducts>();

            Context.ShoppingCarts.Add(newUserCart);

            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            // Check if user that is about to be deleted exists
            if (!(await UserExists(userId)))
            {
                throw new ArgumentException("User does not exist");
            }

            User userToDelete = await GetUser(userId);

            Context.Remove(userToDelete);

            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUser(int userId)
        {
            return await Context.Users.FindAsync(userId);
        }

        public async Task<User> GetUser(string email)
        {
            return await Context.Users.Where(user => user.Email == email).FirstAsync();
        }

        public async Task<bool> UpdateUser(RUserDTO userData)
        {
            if (userData.Id.Value == 0)
            {
                throw new ArgumentException("Undefined user id to update");
            }

            User toUpdateUser = await GetUser(userData.Id.Value);

            if (toUpdateUser == null)
            {
                // Handle the case where the mapping failed
                throw new ArgumentException("Can't find user to update");
            }

            Context.Users.Update(toUpdateUser);

            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UserExists(int userId)
        {
            return await Context.Users.FindAsync(userId) != null;
        }

        public async Task<bool> UserExists(string email)
        {
            return (await Context.Users.Where(user => user.Email == email).FirstOrDefaultAsync()) != null;
        }

    }
}
