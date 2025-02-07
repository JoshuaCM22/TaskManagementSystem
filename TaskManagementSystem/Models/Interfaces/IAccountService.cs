using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.Models.Interfaces
{
    public interface IAccountService
    {
        void Register(RegisterViewModel viewModel);
        bool Login(LoginViewModel viewModel);
        bool IsAdmin(string username);
    }
}
