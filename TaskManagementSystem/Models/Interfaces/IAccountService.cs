using System.Threading.Tasks;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.Models.Interfaces
{
    public interface IAccountService
    {
        Task Register(RegisterViewModel viewModel);
        Task<bool> Login(LoginViewModel viewModel);
        Task<bool> IsAdmin(string username);
        Task<bool> IsUsernameExist(string username);
    }
}
