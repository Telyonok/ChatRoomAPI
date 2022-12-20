using ChatRoomAPI.Data;
using ChatRoomAPI.Models;
using ChatRoomAPI.Repositories;
using Serilog;
using System.Security.Authentication;

namespace ChatRoomAPI.Services
{
    public class UsersService: IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenRepository _tokenRepository;

        public UsersService(IUsersRepository usersRepository, ITokenRepository tokenRepository)
        {
            _usersRepository = usersRepository;
            _tokenRepository = tokenRepository;
        }

        public async Task InsertUserAsync(User user)
        {
            var userId = await _usersRepository.InsertUserAsync(user);
            // send email
            var validationData = Guid.NewGuid();
            await _usersRepository.UpdateUserValidationData(user.Id, validationData);
        }

        public async Task<User> LoginAsync(TokenRequest login)
        {
            var user = await _usersRepository.GetUserByEmailAsync(login.Email);
            if (user == null)
            {
                throw new AuthenticationException();
            }

            var isValid = Helpers.Crypto.VerifyHashedPassword(user.PasswordHash, login.Password);
            if (!isValid)
            {
                throw new AuthenticationException();
            }

            return user;
        }

        public async Task<User> GetUserFromRefreshTokenAsync(string email, string refreshToken)
        {
            var user = await _usersRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new AuthenticationException();
            }

            if (_tokenRepository.GetToken(email, refreshToken) != null)
            {
                return user;
            }

            return null;
        }
    }
}
