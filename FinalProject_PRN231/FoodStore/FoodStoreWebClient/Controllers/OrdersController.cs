using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FoodStoreWebClient.Controllers
{
    public class OrdersController : Controller
    {
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";


        public OrdersController(IConfiguration configuration)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/Orders");
            string strData = await response.Content.ReadAsStringAsync();
            List<Order> listOrders = JsonConvert.DeserializeObject<List<Order>>(strData);
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }
            return View(listOrders);
        }

        public async Task<IActionResult> AdminFillOrder(string startDate, string endDate)
        {
            //HttpResponseMessage response = await client.GetAsync(ApiPort + "api/Orders/Admin/" + startDate + "/" + endDate);
            //string strData = await response.Content.ReadAsStringAsync();
            //List<Order> listOrders = JsonConvert.DeserializeObject<List<Order>>(strData);
            //var session = HttpContext.Session.GetString("loginUser");
            //if (session != null)
            //{
            //    var member = JsonConvert.DeserializeObject<Account>(session);
            //    ViewData["RoleId"] = member.RoleId;
            //    ViewData["Account"] = member;
            //}
            //ViewData["startDate"] = startDate;
            //ViewData["endDate"] = endDate;

            //return View("Index", listOrders);
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/Orders/Admin/" + startDate + "/" + endDate);
            string strData = await response.Content.ReadAsStringAsync();
            List<Order> listOrders = JsonConvert.DeserializeObject<List<Order>>(strData);
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
                try
                {

                    if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
                    {
                        ViewData["Error"] = "Ngày bắt đầu và ngày kết thúc không được để trống.";
                        return View("Index");
                    }

                    DateTime start = DateTime.Parse(startDate);
                    DateTime end = DateTime.Parse(endDate);

                    if (start > end)
                    {
                        ViewData["Error"] = "Ngày bắt đầu không thể lớn hơn ngày kết thúc.";
                        return View("Index");
                    }

                    // Kiểm tra xem có dữ liệu trả về không
                    if (string.IsNullOrEmpty(strData))
                    {
                        ViewData["Error"] = "Không có đơn đặt hàng nào được tìm thấy trong khoảng thời gian này.";
                        return View("Index");
                    }
                    ViewData["startDate"] = startDate;
                    ViewData["endDate"] = endDate;

                    return View("Index", listOrders);
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = "Đã xảy ra lỗi: " + ex.Message;
                    return View("Index"); // Hoặc chuyển hướng đến trang lỗi nếu cần
                }
            }
            return View("Index");

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
