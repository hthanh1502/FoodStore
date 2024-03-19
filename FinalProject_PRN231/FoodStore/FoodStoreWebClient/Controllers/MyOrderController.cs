using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FoodStoreWebClient.Controllers
{
    public class MyOrderController : Controller
    {
        private FoodStoreContext context;
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private readonly IWebHostEnvironment _env;

        public MyOrderController(IConfiguration configuration, FoodStoreContext context, IWebHostEnvironment env)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
            this.context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
                HttpResponseMessage response = await client.GetAsync(ApiPort + "api/Orders/GetByAccountId/" + member.AccountId);
                string strData = await response.Content.ReadAsStringAsync();
                List<Order> listOrders = JsonConvert.DeserializeObject<List<Order>>(strData);
                return View(listOrders);
            }
            return NotFound();
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/OrderDetails/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            List<OrderDetail> listOrderDetails = JsonConvert.DeserializeObject<List<OrderDetail>>(strData);
            if (id == null || listOrderDetails == null)
            {
                return NotFound();
            }
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }
            ViewData["ListOrderDetails"] = listOrderDetails;
            return View(listOrderDetails);
        }
    }
}
