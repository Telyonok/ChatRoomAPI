using ChatRoomAPI.Models;

namespace ChatRoomAPI.Services
{
    public interface IEmailService
    {
        public Task SendConfirmationEmailAsync(User user);
    }
}
