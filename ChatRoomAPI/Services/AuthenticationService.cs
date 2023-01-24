using ChatRoomAPI.Data;
using ChatRoomAPI.Models;
using ChatRoomAPI.Repositories;
using ChatRoomAPI.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChatRoomWeb.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsersService _usersService;
        private readonly CryptoSettings _cryptoSettings;
        private readonly ITokenRepository _tokenRepository;
        public AuthenticationService(IUsersService userManagementService, IOptions<CryptoSettings> cryptoSettings, ITokenRepository tokenRepository)
        {
            _usersService = userManagementService;
            _cryptoSettings = cryptoSettings.Value;
            _tokenRepository = tokenRepository;
        }

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
        {
            //validate refresh token
            var isValid = ValidateRefreshToken(refreshTokenRequest);
            if (isValid) 
            {
                var user = await _usersService.GetUserFromRefreshTokenAsync(refreshTokenRequest.RefreshToken);
                if (user != null) 
                {
                    return CreateToken(user);
                }
            }
            else
            {
                throw new SecurityTokenExpiredException();
            }

            throw new AuthenticationException();
        }

        private bool ValidateRefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            var storedToken = _tokenRepository.GetToken(refreshTokenRequest.RefreshToken);
            if (storedToken != null) 
            {
                if (storedToken.RefreshExpirationDate < DateTime.Now)
                {
                    return false;
                }

                if (storedToken.RefreshToken != refreshTokenRequest.RefreshToken)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public async Task<TokenResponse> RequestTokenAsync(TokenRequest tokenRequest)
        {
            var user = await _usersService.LoginAsync(tokenRequest);
            if (user != null)
            {
                return CreateToken(user);
            }

            throw new AuthenticationException();
        }

        private TokenResponse CreateToken(User user)
        {
            var claim = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cryptoSettings.JwtSigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                issuer: "localhost:7158",
                audience: "localhost:7158",
                claims: claim,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
                );


            var tokenResponse = new TokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = GenerateRefreshToken()
            };

            _tokenRepository.AddToken(new Token { AccessToken = tokenResponse.Token, RefreshToken = tokenResponse.RefreshToken, UserEmail = user.Email, RefreshExpirationDate = DateTime.Now.AddDays(10)});
            return tokenResponse;
        }

        private string GenerateRefreshToken()
        {
            var buffer = new byte[64];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(buffer);
                return Convert.ToBase64String(buffer);
            }
        }
    }
}
