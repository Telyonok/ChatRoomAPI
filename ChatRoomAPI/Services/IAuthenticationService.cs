using ChatRoomAPI.Models;

namespace ChatRoomAPI.Services
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> RequestTokenAsync(TokenRequest tokenRequest);
    }
}
