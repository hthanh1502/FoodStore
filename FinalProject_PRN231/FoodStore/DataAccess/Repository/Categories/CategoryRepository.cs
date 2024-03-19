using BusinessObjects.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        public void CreateCategory(Category p)
        {
            CategoryDAO.CreateCategory(p);
        }

        public void DeleteCategory(Category p)
        {
            CategoryDAO.DeleteCategory(p);
        }

        public List<Category> GetAllCategories()
        {
            return CategoryDAO.GetAllCategories();
        }

        public Category GetCategoryById(int id)
        {
            return CategoryDAO.GetCategoryById(id);
        }

        public void UpdateCategory(Category p)
        {
            CategoryDAO.UpdateCategory(p);
        }
    }
}
