using TaskManagementSystem.Models.DatabaseModels;

namespace TaskManagementSystem.Models.Interfaces
{
    public interface IAccountRepository
    {
        int CreateAccount(Users users);
        bool VerifyAccount(Users users);
        Users GetUsers(string username);
    }
}