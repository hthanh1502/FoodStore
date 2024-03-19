using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Headers;

namespace FoodStoreWebClient.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";

        public CheckoutController(IConfiguration configuration)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }

        public IActionResult Index()
        {
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }
            return View();
        }

        public async Task<IActionResult> SaveCartItemToDB(OrderDetailDTO request)
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/OrderDetails", RestSharp.Method.Post);
                requesrUrl.AddHeader("content-type", "application/json");
                var body = new OrderDetail
                {
                    OrderId = request.OrderId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    Price = request.Price,
                    Discount = request.Discount
                };
                requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(requesrUrl);
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }


        public async Task<int> SaveCartToDB(OrderDTO request)
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Orders", RestSharp.Method.Post);
                requesrUrl.AddHeader("content-type", "application/json");
                var body = new Order
                {
                    AccountId = request.AccountId,
                    OrderDate = DateTime.Now,
                    TotalAmount = request.TotalAmount
                };
                requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(requesrUrl);
                var order = JsonConvert.DeserializeObject<Order>(response.Content);
                return order.OrderId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ActionResult> Checkout()
        {
            var sessionUser = HttpContext.Session.GetString("loginUser");
            var cart = GetCartItems();
            if (sessionUser != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(sessionUser);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
                var currentUser = JsonConvert.DeserializeObject<Account>(sessionUser);
                if (currentUser != null)
                {
                    var orderId = SaveCartToDB(new OrderDTO
                    {
                        AccountId = currentUser.AccountId,
                        OrderDate = DateTime.Now,
                        TotalAmount = CalculatorCart(cart)
                    }).Result;
                    foreach (var item in cart)
                    {
                        var od = new OrderDetailDTO
                        {
                            OrderId = orderId,
                            ProductId = item.product.ProductId,
                            Quantity = item.quantity,
                            Price = item.product.Price,
                            Discount = item.product.Discount

                        };
                        SaveCartItemToDB(od);
                    }
                    MinusProductQuantity();
                    ClearCart();
                    return View("Index", "Checkout");
                }
            }
            return View("Index", "Login");
        }

        public async Task<IActionResult> MinusProductQuantity()
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Products/Checkout", RestSharp.Method.Post);
                requesrUrl.AddHeader("content-type", "application/json");
                var body = GetCartItems();
                var listCategory = GetAllCategory().Result;
                foreach (var item in body)
                {
                    item.product.Category = listCategory.Where(x => x.CategoryId == item.product.CategoryId).FirstOrDefault();
                }
                requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(requesrUrl);
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<List<Category>> GetAllCategory()
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/categories");
            string strData = await response.Content.ReadAsStringAsync();
            List<Category> listCategories = JsonConvert.DeserializeObject<List<Category>>(strData);
            return listCategories;
        }

        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";


        // Lấy cart từ Session (danh sách CartItem)
        List<CartItem> GetCartItems()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        decimal CalculatorCart(List<CartItem> list)
        {
            decimal result = 0;
            foreach (var item in list)
            {
                result += (decimal)(1 - item.product.Discount) * item.product.Price * decimal.Parse(item.quantity.ToString());
            }
            return result;
        }
    }
}
