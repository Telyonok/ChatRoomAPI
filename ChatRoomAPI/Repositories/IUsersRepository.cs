using ChatRoomAPI.Models;

namespace ChatRoomAPI.Repositories
{
    public interface IUsersRepository
    {
        Task InsertUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
    }
}
