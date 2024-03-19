using BusinessObjects.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Roles
{
    public class RoleRepository : IRoleRepository
    {
        public List<Role> GetAllRoles()
        {
            return RoleDAO.GetAllRoles();
        }

        public Role GetRoleById(int id)
        {
            return RoleDAO.GetRoleById(id);
        }
    }
}
