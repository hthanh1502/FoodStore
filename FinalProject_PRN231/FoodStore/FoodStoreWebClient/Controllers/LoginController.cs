using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FoodStoreWebClient.Controllers
{
    public class LoginController : Controller
    {
        private readonly FoodStoreContext _context;
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        public LoginController(FoodStoreContext context, IConfiguration configuration)
        {
            _context = context;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }

        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost, ActionName("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            Account account;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Accounts/Login", RestSharp.Method.Post);
                requesrUrl.AddHeader("content-type", "application/json");
                var body = new LoginRequest
                {
                    Email = request.Email,
                    Password = request.Password
                };
                requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(requesrUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var acc = JsonSerializer.Deserialize<Account>(response.Content, options);
                    account = new Account
                    {
                        AccountId = acc.AccountId,
                        Email = acc.Email,
                        Password = acc.Password,
                        Fullname = acc.Fullname,
                        Avatar = acc.Avatar,
                        Dob = acc.Dob,
                        Gender = acc.Gender,
                        Phone = acc.Phone,
                        Address = acc.Address,
                        RoleId = acc.RoleId,

                    };
                    var loginUser = JsonSerializer.Serialize(account);
                    HttpContext.Session.SetString("loginUser", loginUser);
                    ViewData["user"] = account.Email;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["err"] = "Login failed!";
                    return View("Index");
                }

            }
            catch (Exception)
            {
                throw;
            }
            //return Ok("Tai khoan hoac mat khau ban nhap sai !!!");
        }
    }
}
