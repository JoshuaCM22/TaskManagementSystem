using System;
using System.Data.Entity;
using System.Threading.Tasks;
using TaskManagementSystem.Context;
using TaskManagementSystem.Models.DatabaseModels;
using TaskManagementSystem.Models.Interfaces;

namespace TaskManagementSystem.Models.DataAccessLayer
{
    public class AccountRepository : IAccountRepository, IDisposable
    {
        private readonly DBContext _dbContext;
        public AccountRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAccount(Users users)
        {
            _dbContext.Users.Add(users);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> VerifyAccount(Users users)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(s => s.Username == users.Username);
            if (user == null) return false;
            if (user.Password == users.Password) return true;
            return false;
        }

        public async Task<bool> IsAdmin(string username)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(s => s.Username == username);
            return (user.RoleID == 1);
        }

        public async Task<bool> IsUsernameExist(string username)
        {
            int count = await _dbContext.Users.CountAsync(x => x.Username == username);
            return (count > 0);
        }

        // Dispose this context when this repository is no longer needed. To avoid increased memory usage.
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}