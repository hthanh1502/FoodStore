using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class AccountDAO
    {
        public static Account GetAccountByEmail(string email)
        {
            var account = new Account();
            try
            {
                using (var connection = new FoodStoreContext())
                {
                    account = connection.Accounts.FirstOrDefault(x => x.Email.Equals(email));
                }
            }
            catch (Exception)
            {

                throw;
            }
            return account;
        }
        public static Account Login(string email, string password)
        {
            var account = new Account();
            try
            {
                using (var connection = new FoodStoreContext())
                {
                    account = connection.Accounts.FirstOrDefault(x => x.Email.Equals(email) && x.Password.Equals(password));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return account;
        }
        public static List<Account> GetAllAccount()
        {
            var listAccount = new List<Account>();
            try
            {
                using (var connection = new FoodStoreContext())
                {
                    listAccount = connection.Accounts.ToList();
                }
                foreach (var item in listAccount)
                {
                    item.Role = RoleDAO.GetRoleById(item.RoleId);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return listAccount;
        }
        public static Account GetAccountById(int id)
        {
            var account = new Account();
            try
            {
                using (var connection = new FoodStoreContext())
                {
                    account = connection.Accounts.SingleOrDefault(x => x.AccountId == id);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return account;
        }
        public static void CreateAccount(Account acc)
        {
            try
            {
                using (var context = new FoodStoreContext())
                {
                    context.Accounts.Add(acc);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void UpdateAccount(Account acc)
        {
            try
            {
                using (var context = new FoodStoreContext())
                {
                    context.Entry<Account>(acc).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void DeleteAccount(Account acc)
        {
            try
            {
                using (var context = new FoodStoreContext())
                {
                    var a = context.Accounts.SingleOrDefault(c => c.AccountId == acc.AccountId);
                    context.Accounts.Remove(a);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
    }
}