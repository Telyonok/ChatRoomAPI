using ChatRoomAPI.Models;

namespace ChatRoomAPI.Repositories
{
    public interface IUsersRepository
    {
        Task<int> InsertUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task UpdateUserValidationData(int userId, Guid validationData);
    }
}
