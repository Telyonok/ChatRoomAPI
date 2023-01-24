using ChatRoomAPI.Data;
using ChatRoomAPI.Models;
using ChatRoomWeb.Data;

namespace ChatRoomAPI.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger logger;
        public TokenRepository(IServiceScopeFactory scopeFactory, ILogger<UsersRepository> logger)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        public void AddToken(Token token)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (!_db.Tokens.Contains(token))
                {
                    _db.Tokens.Add(token);
                }
                else
                {
                    _db.Tokens.Update(token);
                }
                _db.SaveChanges();
                logger.LogDebug($"Token for email {token.UserEmail} was updated.\n");
            }
        }

        public Token GetToken(string refreshToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var token = _db.Tokens
                    .Where(t => t.RefreshToken == refreshToken)
                    .FirstOrDefault();
                if (token != null) 
                {
                    return token;
                }

                return null;
            }
        }
    }
}
