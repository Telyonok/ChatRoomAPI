using ChatRoomAPI.Models;

namespace ChatRoomAPI.Repositories
{
    public interface IUsersRepository
    {
        Task<int> InsertUserAsync(User user);
        Task<int> GetUserIdByEmailAsync(string email);
        Task UpdateUserValidationData(int userId, Guid validationData);
        Task<int> GetUserIdByVerificationAsync(string verificationData);
        Task<int> GetUserIdByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
    }
}
