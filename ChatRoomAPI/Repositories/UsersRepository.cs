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

        public async Task<int> GetUserIdByEmailAsync(string email)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var user = _db.Users.Where(x => x.Email.ToLower() == email.ToLower()).FirstOrDefault();
                if (user == null)
                {
                    return -1;
                }

                return user.Id;
            }
        }

        public async Task<int> GetUserIdByUsernameAsync(string username)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var user = _db.Users.Where(x => x.Username.ToLower() == username.ToLower()).FirstOrDefault();
                if (user == null)
                {
                    return -1;
                }

                return user.Id;
            }
        }

        public async Task<int> GetUserIdByVerificationAsync(string verificationData)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var user = _db.Users.Where(x => x.VerificationData.ToString() == verificationData).FirstOrDefault();
                if (user == null)
                {
                    return -1;
                }

                user.IsConfirmed = true;
                user.VerificationData = null;
                _db.Update(user);
                await _db.SaveChangesAsync();
                return user.Id;
            }
        }

        public async Task<int> InsertUserAsync(User user)
        {    
            using (var scope = scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
                logger.LogDebug($"User {user.Username} with:\nid {user.Id}\nemail {user.Email}\ncreation date {user.CreationDate}\nwas added to database.\n");
                return user.Id;
            }
        }

        public async Task UpdateUserValidationData(int userId, Guid verificationData)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var user = _db.Users.Where(u => u.Id == userId).FirstOrDefault();
                if (user != null)
                {
                    user.VerificationData = verificationData;
                    _db.Users.Update(user);
                    await _db.SaveChangesAsync();
                }
                logger.LogDebug($"User {user.Username} with:\nid {user.Id}\nemail {user.Email}\ncreation date {user.CreationDate}\nwas added to database.\n");
            }
        }
    }
}
