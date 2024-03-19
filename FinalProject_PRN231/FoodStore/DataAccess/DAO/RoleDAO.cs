using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class RoleDAO
    {
        public static List<Role> GetAllRoles()
        {
            var listRoles = new List<Role>();
            try
            {
                using (var context = new FoodStoreContext())
                {
                    listRoles = context.Roles.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listRoles;
        }

        public static Role GetRoleById(int id)
        {
            var role = new Role();
            try
            {
                using (var connection = new FoodStoreContext())
                {
                    role = connection.Roles.SingleOrDefault(x => x.RoleId == id);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return role;
        }
    }
}
