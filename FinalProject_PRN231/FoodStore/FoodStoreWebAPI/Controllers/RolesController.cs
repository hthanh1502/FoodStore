using BusinessObjects.Models;
using DataAccess.Repository.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private IRoleRepository _repo;

        public RolesController(IRoleRepository repository)
        {
            _repo = repository;
        }

        // GET: api/Categories
        [HttpGet]
        public ActionResult<IEnumerable<Role>> GetAllCategories()
        {
            if (_repo.GetAllRoles().Count == 0)
            {
                return NotFound();
            }
            return _repo.GetAllRoles();
        }
    }
}
