using ChatRoomAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomAPI.Services
{
    public interface IUsersService
    {
        Task InsertUserAsync(User user);
        Task<User> LoginAsync(TokenRequest login);
        Task<User> GetUserFromRefreshTokenAsync(string refreshToken);
        Task<int> GetUserIdByVerificationAsync(string verificationData);
        Task<bool> IsUniqueEmailAsync(string email);
        Task<bool> IsUniqueUsernameAsync(string username);
    }
}
