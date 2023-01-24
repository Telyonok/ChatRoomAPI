using ChatRoomAPI.Data;
using ChatRoomAPI.Models;
using ChatRoomAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Serilog;
using System.Security.Authentication;

namespace ChatRoomAPI.Services
{
    public class UsersService: IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IEmailService _emailService;

        public UsersService(IUsersRepository usersRepository, ITokenRepository tokenRepository, IEmailService emailService)
        {
            _usersRepository = usersRepository;
            _tokenRepository = tokenRepository;
            _emailService = emailService;
        }

        public async Task InsertUserAsync(User user)
        {
            if (!IsUniqueEmailAsync(user.Email).Result)
            {
                throw new AuthenticationException("Email is already taken");
            }
            if (IsUniqueUsernameAsync(user.Username).Result)
            {
                throw new AuthenticationException("Username is already taken");
            }

            var userId = await _usersRepository.InsertUserAsync(user);
            // send email
            var validationData = Guid.NewGuid();
            await _usersRepository.UpdateUserValidationData(user.Id, validationData);
            //await _emailService.SendConfirmationEmailAsync(user);
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

        public async Task<User> GetUserFromRefreshTokenAsync(string refreshToken)
        {
            var token = _tokenRepository.GetToken(refreshToken);
            
            if (token == null)
            {
                return null;
            }

            var email = token.UserEmail;

            var user = await _usersRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new AuthenticationException();
            }
            
            return user;
        }

        public async Task<int> GetUserIdByVerificationAsync(string verificationData)
        {
            var userId = await _usersRepository.GetUserIdByVerificationAsync(verificationData);
            return userId;
        }

        public async Task<bool> IsUniqueEmailAsync(string email)
        {
            var userId = await _usersRepository.GetUserIdByEmailAsync(email);
            return userId < 0;
        }

        public async Task<bool> IsUniqueUsernameAsync(string username)
        {
            var userId = await _usersRepository.GetUserIdByUsernameAsync(username);
            return userId < 0;
        }
    }
}
