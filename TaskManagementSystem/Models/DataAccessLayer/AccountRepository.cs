using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
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
            return (user.Password == users.Password);
        }


        public async Task<bool> IsAdmin(string username)
        {
            return await (from u in _dbContext.Users
                          where u.Username == username
                          && u.RoleID == 1
                          select u).AnyAsync();
        }

        public async Task<bool> IsUsernameExist(string username)
        {
            return await (from u in _dbContext.Users
                          where u.Username == username
                          select u).AnyAsync();
        }

        // Dispose this context when this repository is no longer needed. To avoid increased memory usage.
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}