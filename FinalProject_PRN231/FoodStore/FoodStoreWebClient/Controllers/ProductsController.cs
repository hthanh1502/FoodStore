using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Headers;
using FoodStoreWebClient.Filters;

namespace FoodStoreWebClient.Controllers
{
    public class ProductsController : Controller
    {
        private FoodStoreContext context;
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private readonly IWebHostEnvironment _env;

        public ProductsController(IConfiguration configuration, FoodStoreContext context, IWebHostEnvironment env)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
            this.context = context;
            _env = env;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/products");
            string strData = await response.Content.ReadAsStringAsync();
            Console.WriteLine(strData);
            List<Product> listProducts = JsonConvert.DeserializeObject<List<Product>>(strData);
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }
            return View(listProducts);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/products/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            Console.WriteLine(strData);
            Product product = JsonConvert.DeserializeObject<Product>(strData);
            if (id == null || product == null)
            {
                return NotFound();
            }
            var categories = getAllCategory().Result.ToList();
            product.Category = categories.Where(x => x.CategoryId == product.CategoryId).FirstOrDefault();
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }
            return View(product);
        }

        public async Task<List<Category>> getAllCategory()
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/categories");
            string strData = await response.Content.ReadAsStringAsync();
            Console.WriteLine(strData);
            var listCategories = JsonConvert.DeserializeObject<List<Category>>(strData);
            return listCategories;
        }


        //GET: Products/Create
        [CustomAuthorizationFilter]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(context.Categories, "CategoryId", "CategoryName");
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }
            return View();
        }


        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductImage,ProductDescription,CategoryId,Price,Discount,Quantity,Active")] Product product, IFormFile imgfile)
        {
            try
            {
                // upload file ảnh
                MultipartFormDataContent formData = new MultipartFormDataContent();
                StreamContent fileContent = new StreamContent(imgfile.OpenReadStream());
                formData.Add(fileContent, "file", imgfile.FileName);
                string filePath = Path.Combine("wwwroot/img/Products", imgfile.FileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imgfile.CopyToAsync(fileStream);
                }
                product.ProductImage = imgfile.FileName;

                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/products", RestSharp.Method.Post);
                requesrUrl.AddHeader("content-type", "application/json");
                var body = new Product
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductImage = product.ProductImage,
                    Price = product.Price,
                    Discount = product.Discount,
                    CategoryId = product.CategoryId,
                    Quantity = product.Quantity,
                    Active = product.Active
                };
                requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(requesrUrl);
            }
            catch (Exception)
            {
                throw;
            }
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductImage,ProductDescription,CategoryId,Price,Discount,Quantity,Active")] ProductDTO product, IFormFile imgfile)
        {
            try
            {
                // upload file ảnh
                MultipartFormDataContent formData = new MultipartFormDataContent();
                StreamContent fileContent = new StreamContent(imgfile.OpenReadStream());
                formData.Add(fileContent, "file", imgfile.FileName);
                string filePath = Path.Combine("wwwroot/img/Products", imgfile.FileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imgfile.CopyToAsync(fileStream);
                }
                //product.ProductImage = imgfile.FileName;

                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/products/{id}", RestSharp.Method.Put);
                requesrUrl.AddHeader("content-type", "application/json");
                var body = new ProductDTO
                {
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductImage = imgfile.FileName,
                    Price = product.Price,
                    Discount = product.Discount,
                    CategoryId = product.CategoryId,
                    Quantity = product.Quantity,
                    Active = product.Active
                };
                requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(requesrUrl);
            }
            catch (Exception)
            {
                throw;
            }
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/products/delete/{id}", RestSharp.Method.Post);
                requesrUrl.AddHeader("content-type", "application/json");
                var body = new Product();
                requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(requesrUrl);
            }
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
