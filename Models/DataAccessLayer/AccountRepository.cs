using System;
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

        public int CreateAccount(Users users)
        {
            _dbContext.Users.Add(users);
            return _dbContext.SaveChanges();            
        }


        public bool VerifyAccount(Users users)
        {
            var user = _dbContext.Users.ToList().SingleOrDefault(s => s.Username == users.Username);
            if (user == null) return false;
            if (user.Password == users.Password) return true;
            return false;
        }



        public Users GetUsers(string username)
        {
            return _dbContext.Users.SingleOrDefault(x => x.Username == username);
        }

        // Dispose this context when this repository is no longer needed. To avoid increased memory usage.
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}