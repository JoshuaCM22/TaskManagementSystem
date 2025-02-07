using TaskManagementSystem.Models.DatabaseModels;

namespace TaskManagementSystem.Models.Interfaces
{
    public interface IAccountRepository
    {
        void CreateAccount(Users users);
        bool VerifyAccount(Users users);
        Users GetUsers(string username);
        bool IsAdmin(string username);
    }
}