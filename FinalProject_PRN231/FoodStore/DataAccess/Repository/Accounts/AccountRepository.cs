using BusinessObjects.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Accounts
{
    public class AccountRepository : IAccountRepository
    {
        public void CreateAccount(Account a)
        {
            AccountDAO.CreateAccount(a);
        }

        public void DeleteAccount(Account a)
        {
            AccountDAO.DeleteAccount(a);
        }

        public Account GetAccountByEmail(string email)
        {
            return AccountDAO.GetAccountByEmail(email);
        }
        public Account GetAccountById(int id)
        {
            return AccountDAO.GetAccountById(id);
        }

        public List<Account> GetAllAccount()
        {
            return AccountDAO.GetAllAccount();
        }

        public Account Login(string email, string password)
        {
            return AccountDAO.Login(email, password);
        }

        public void UpdateAccount(Account a)
        {
            AccountDAO.UpdateAccount(a);
        }
    }
}
