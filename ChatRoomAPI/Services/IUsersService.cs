using ChatRoomAPI.Models;

namespace ChatRoomAPI.Services
{
    public interface IUsersService
    {
        Task InsertUserAsync(User user);
        Task<User> LoginAsync(TokenRequest login);
    }
}
