using TaskManagementSystem.Models.DatabaseModels;
using TaskManagementSystem.Models.Interfaces;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.Models.BusinessLogicLayer
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public void Register(RegisterViewModel viewModel)
        {
            Users user = new Users() { Username = viewModel.Username, Password = viewModel.Password, RoleID = viewModel.RoleId };
             _accountRepository.CreateAccount(user);
        }


        public bool Login(LoginViewModel viewModel)
        {
            Users user = new Users() { Username = viewModel.Username, Password = viewModel.Password };
            return _accountRepository.VerifyAccount(user);
        }

        public bool IsAdmin(string username)
        {
            return _accountRepository.IsAdmin(username);
        }

    }
}