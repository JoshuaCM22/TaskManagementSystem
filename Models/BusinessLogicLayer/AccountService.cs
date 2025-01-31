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

        public bool Register(RegisterViewModel viewModel)
        {
            Users user = new Users() { Username = viewModel.Username, Password = viewModel.Password, RoleID = viewModel.RoleId };
            int i = _accountRepository.CreateAccount(user);
            if (i > 0) return true;
            return false;
        }


        public bool Login(LoginViewModel viewModel)
        {
            Users user = new Users() { Username = viewModel.Username, Password = viewModel.Password };
            return _accountRepository.VerifyAccount(user);
        }

    }
}