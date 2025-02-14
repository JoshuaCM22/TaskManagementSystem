using System;
using System.Threading.Tasks;
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

        public async Task Register(RegisterViewModel viewModel)
        {
            Users user = new Users() { Username = viewModel.Username, Password = viewModel.Password, RoleID = 2 }; // 2 = Regular User
            await _accountRepository.CreateAccount(user);
        }


        public async Task<bool> Login(LoginViewModel viewModel)
        {
            Users user = new Users() { Username = viewModel.Username, Password = viewModel.Password };
            return await _accountRepository.VerifyAccount(user);
        }

        public async Task<bool> IsAdmin(string username)
        {
            return await _accountRepository.IsAdmin(username);
        }

        public async Task<bool> IsUsernameExist(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new Exception("The username cannot be null or empty string.");
            return await _accountRepository.IsUsernameExist(username.Trim());
        }

    }
}