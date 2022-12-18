using ChatRoomAPI.Models;
using ChatRoomAPI.Repositories;
using System.Security.Authentication;

namespace ChatRoomAPI.Services
{
    public class UsersService: IUsersService
    {
        private IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task InsertUserAsync(User user)
        {
            await _usersRepository.InsertUserAsync(user);
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
    }
}
