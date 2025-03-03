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
            if (string.IsNullOrEmpty(viewModel.Username)) throw new ArgumentException("The username cannot be null or empty string.");
            else if (string.IsNullOrEmpty(viewModel.Password)) throw new ArgumentException("The password cannot be null or empty string.");

            Users user = new Users() { Username = viewModel.Username.Trim(), Password = viewModel.Password, RoleID = 2 }; // 2 = Regular User
            await _accountRepository.CreateAccount(user);
        }


        public async Task<bool> Login(LoginViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Username)) throw new ArgumentException("The username cannot be null or empty string.");
            else if (string.IsNullOrEmpty(viewModel.Password)) throw new ArgumentException("The password cannot be null or empty string.");

            Users user = new Users() { Username = viewModel.Username.Trim(), Password = viewModel.Password };
            return await _accountRepository.VerifyAccount(user);
        }

        public async Task<bool> IsAdmin(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("The username cannot be null or empty string.");
            return await _accountRepository.IsAdmin(username.Trim());
        }

        public async Task<bool> IsUsernameExist(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("The username cannot be null or empty string.");
            return await _accountRepository.IsUsernameExist(username.Trim());
        }

    }
}