using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Headers;

namespace FoodStoreWebClient.Controllers
{
    public class ProfileController : Controller
    {
        private FoodStoreContext context;
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private readonly IWebHostEnvironment _env;

        public ProfileController(IConfiguration configuration, FoodStoreContext context, IWebHostEnvironment env)
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
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                var account = GetAccountById(member.AccountId);
                ViewData["Account"] = account.Result;
                return View();
            }
            return NotFound();

        }

        public async Task<Account> GetAccountById(int id)
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/accounts/byId/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            Account account = JsonConvert.DeserializeObject<Account>(strData);
            return account;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile([Bind("AccountId,Email,Password,Fullname,Avatar,Dob,Gender,Phone,Address,RoleId")] AccountDTO account, IFormFile avtFile)
        {
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;

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

                    RestClient client = new RestClient(ApiPort);
                    var requesrUrl = new RestRequest($"api/Accounts/" + member.AccountId, RestSharp.Method.Put);
                    requesrUrl.AddHeader("content-type", "application/json");
                    var body = new Account
                    {
                        Email = account.Email,
                        Password = account.Password,
                        Fullname = account.Fullname,
                        Avatar = avtFile.FileName,
                        Dob = account.Dob,
                        Gender = account.Gender,
                        Phone = account.Phone,
                        Address = account.Address,
                        RoleId = account.RoleId,
                    };
                    requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                    var response = await client.ExecuteAsync(requesrUrl);

                    ViewData["UpdateMessage"] = "Update Successfully!!";
                    return View("Index");
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return NotFound();

        }
    }
}
