using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.Models.Interfaces
{
    public interface IAccountService
    {
        bool Register(RegisterViewModel viewModel);
        bool Login(LoginViewModel viewModel);
    }
}
