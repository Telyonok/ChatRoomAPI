using ChatRoomAPI.Data;
using ChatRoomAPI.Models;

namespace ChatRoomAPI.Repositories
{
    public interface ITokenRepository
    {
        void AddToken(Token token);
        Token GetToken(string refreshToken);
    }
}
