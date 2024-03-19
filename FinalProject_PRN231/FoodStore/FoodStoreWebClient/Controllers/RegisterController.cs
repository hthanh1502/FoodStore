using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FoodStoreWebClient.Controllers
{
    public class RegisterController : Controller
    {
        private FoodStoreContext context;
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private readonly IWebHostEnvironment _env;

        public RegisterController(IConfiguration configuration, FoodStoreContext context, IWebHostEnvironment env)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
            this.context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<Account> GetAccountByEmail(string email)
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/accounts/ByEmail/" + email);
            string strData = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Account account = JsonSerializer.Deserialize<Account>(strData, options);
            return account;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("AccountId,Email,Password,Fullname,Avatar,Dob,Gender,Phone,Address,RoleId")] Account account, IFormFile avtFile)
        {
            // kiểm tra xem email của tk mới đã tồn tại chưa
            Account acc = GetAccountByEmail(account.Email).Result;
            if (acc != null)
            {
                ViewData["Error"] = "Email đã tồn tại!!!";
                return View("Index");
            }
            else
            {
                try
                {
                    // upload file ảnh
                    MultipartFormDataContent formData = new MultipartFormDataContent();
                    StreamContent fileContent = new StreamContent(avtFile.OpenReadStream());
                    formData.Add(fileContent, "file", avtFile.FileName);
                    string filePath = Path.Combine("wwwroot/img/Avatars", avtFile.FileName);
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await avtFile.CopyToAsync(fileStream);
                    }
                    account.Avatar = avtFile.FileName;

                    RestClient client = new RestClient(ApiPort);
                    var requesrUrl = new RestRequest($"api/Accounts/Register", RestSharp.Method.Post);
                    requesrUrl.AddHeader("content-type", "application/json");
                    var body = new RegisterDTO
                    {
                        Email = account.Email,
                        Password = account.Password,
                        Fullname = account.Fullname,
                        Avatar = account.Avatar,
                        Dob = account.Dob,
                        Gender = account.Gender,
                        Phone = account.Phone,
                        Address = account.Address,
                    };
                    requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                    var response = await client.ExecuteAsync(requesrUrl);

                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
