using BusinessObjects.Models;
using DataAccess.Repository.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryRepository _repo;

        public CategoriesController(ICategoryRepository repository)
        {
            _repo = repository;
        }

        // GET: api/Categories
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAllCategories()
        {
            if (_repo.GetAllCategories().Count == 0)
            {
                return NotFound();
            }
            return _repo.GetAllCategories();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public ActionResult<Category> GetCategoryById(int id)
        {
            if (_repo.GetAllCategories().Count == 0)
            {
                return NotFound();
            }
            var category = _repo.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }
            return category;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, Category category)
        {
            var cate = _repo.GetCategoryById(id);
            if (cate == null)
            {
                return NotFound();
            }
            cate.CategoryName = category.CategoryName;
            _repo.UpdateCategory(cate);
            return NoContent();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Category> CreateCategory(Category category)
        {
            var cate = new Category
            {
                CategoryName = category.CategoryName
            };
            _repo.CreateCategory(cate);
            return NoContent();
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var cate = _repo.GetCategoryById(id);
            if (cate == null)
            {
                return NotFound();
            }
            _repo.DeleteCategory(cate);
            return NoContent();
        }
    }
}
