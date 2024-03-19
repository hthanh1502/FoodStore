using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using DataAccess.Repository.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            if (_repo.GetAllProducts().Count == 0)
            {
                return NotFound();
            }
            return _repo.GetAllProducts();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(int id)
        {
            if (_repo.GetAllProducts().Count == 0)
            {
                return NotFound();
            }
            var product = _repo.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductDTO product)
        {
          
            var pro = _repo.GetProductById(id);
            if (pro == null)
            {
                return NotFound();
            }
            pro.ProductName = product.ProductName;
            pro.ProductDescription = product.ProductDescription;
            pro.ProductImage = product.ProductImage;
            pro.Price = product.Price;
            pro.CategoryId = product.CategoryId;
            pro.Discount = product.Discount;
            pro.Quantity = product.Quantity;
            pro.Active = product.Active;

            _repo.UpdateProduct(pro);
            return NoContent();
        }

        [HttpPost]
        public ActionResult<Product> CreateProduct(ProductDTO productdto)
        {
            if (productdto.Price < 0)
            {
                return BadRequest("Price cannot be a negative number.");
            }

            var pro = new ProductDTO
            {
                ProductName = productdto.ProductName,
                ProductDescription = productdto.ProductDescription,
                ProductImage = productdto.ProductImage,
                CategoryId = productdto.CategoryId,
                Price = productdto.Price,
                Discount = productdto.Discount,
                Quantity = productdto.Quantity,
                Active = productdto.Active
            };
            _repo.CreateProduct(pro);
            return NoContent();
        }

        [HttpPost("Checkout")]
        public ActionResult<Product> Checkout(List<CartItem> list)
        {
            _repo.Checkout(list);
            return NoContent();
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("delete/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var pro = _repo.GetProductById(id);
            if (pro == null)
            {
                return NotFound();
            }
           
            pro.Active = false;
            _repo.UpdateProduct(pro);
           
            return NoContent();
        }

        [HttpGet("{search}/{categoryId}/{sort}/{pageIndex}/{pageSize}")]
        public async Task<ProductResponseDTO> RequestProduct(string? search, int categoryId, int sort, int pageIndex, int pageSize)
        {
            var productRequest = new ProductRequestDTO
            {
                search = string.IsNullOrEmpty(search) ? "all" : search,
                categoryId = categoryId,
                sort = sort,
                pageIndex = pageIndex,
                pageSize = pageSize,
            };

            var response = _repo.RequestProduct(productRequest);
            if (response == null)
            {
                return null;
            }
            return response;
        }
       

    }
}
