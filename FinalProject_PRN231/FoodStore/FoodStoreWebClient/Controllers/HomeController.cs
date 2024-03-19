using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using FoodStoreWebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace FoodStoreWebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private FoodStoreContext context;
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, FoodStoreContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
            this.context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(string search, int categoryId, int sort, int pageIndex, int pageSize)
        {
            if (String.IsNullOrEmpty(search))
            {
                search = "all";
            }
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            if (pageSize == 0)
            {
                pageSize = 12;
            }
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }

            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/Products/" + search + "/" + categoryId + "/" + sort + "/" + pageIndex + "/" + pageSize);
            string strData = await response.Content.ReadAsStringAsync();
            Console.WriteLine(strData);
            ProductResponseDTO listProducts = JsonConvert.DeserializeObject<ProductResponseDTO>(strData);

            HttpResponseMessage res = await client.GetAsync(ApiPort + "api/categories");
            string strCate = await res.Content.ReadAsStringAsync();
            Console.WriteLine(strCate);
            List<Category> listCategories = JsonConvert.DeserializeObject<List<Category>>(strCate);

            ViewData["search"] = search;
            ViewData["categoryId"] = categoryId;
            ViewData["sort"] = sort;
            ViewData["pageIndex"] = pageIndex;
            ViewData["pageSize"] = pageSize;
            ViewData["ListCategories"] = listCategories;
            ViewData["Response"] = listProducts;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}