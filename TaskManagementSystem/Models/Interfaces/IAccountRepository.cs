using System.Threading.Tasks;
using TaskManagementSystem.Models.DatabaseModels;

namespace TaskManagementSystem.Models.Interfaces
{
    public interface IAccountRepository
    {
        Task CreateAccount(Users users);
        Task<bool> VerifyAccount(Users users);
        Task<bool> IsAdmin(string username);
        Task<bool> IsUsernameExist(string username);
    }
}