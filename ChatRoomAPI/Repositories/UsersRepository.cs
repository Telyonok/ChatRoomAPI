using ChatRoomAPI.Models;
using ChatRoomWeb.Data;
using Microsoft.AspNetCore.Identity;

namespace ChatRoomAPI.Repositories
{
    public class UsersRepository: IUsersRepository
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger logger;
        public UsersRepository(IServiceScopeFactory scopeFactory, ILogger<UsersRepository> logger)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var user = _db.Users.Where(x => x.Email == email).FirstOrDefault();
                if (user != null)
                {
                    return user;
                }

                return null;
            }
        }

        public async Task InsertUserAsync(User user)
        {    
            using (var scope = scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
                logger.LogDebug($"User {user.Username} with:\nid {user.Id}\nemail {user.Email}\ncreation date {user.CreationDate}\nwas added to database.\n");
            }
        }
    }
}
