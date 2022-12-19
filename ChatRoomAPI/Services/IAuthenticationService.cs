using ChatRoomAPI.Models;

namespace ChatRoomAPI.Services
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> RequestTokenAsync(TokenRequest tokenRequest);
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    }
}
