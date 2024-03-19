using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Accounts
{
    public interface IAccountRepository
    {
        void CreateAccount(Account a);
        Account GetAccountByEmail(string email);
        Account GetAccountById(int id);
        Account Login(string email, string password);
        void DeleteAccount(Account a);
        void UpdateAccount(Account a);
        List<Account> GetAllAccount();
    }
}
