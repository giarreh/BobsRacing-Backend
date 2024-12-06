﻿using Bobs_Racing.Models;

namespace Bobs_Racing.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserCredentialsAsync(User user);
        Task UpdateUserCreditsAsync(User user);

        Task DeleteUserAsync(int id);

        /*// Check if a username is already taken
        Task<bool> IsUsernameTakenAsync(string username);*/
    }
}
