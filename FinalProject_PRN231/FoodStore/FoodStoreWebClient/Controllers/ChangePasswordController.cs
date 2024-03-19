using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Headers;

namespace FoodStoreWebClient.Controllers
{
    public class ChangePasswordController : Controller
    {
        private FoodStoreContext context;
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private readonly IWebHostEnvironment _env;

        public ChangePasswordController(IConfiguration configuration, FoodStoreContext context, IWebHostEnvironment env)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword([Bind("oldPass,newPass,reNewPass")] ChangePasswordDTO request)
        {
            try
            {
                var session = HttpContext.Session.GetString("loginUser");
                if (session != null)
                {
                    var member = JsonConvert.DeserializeObject<Account>(session);
                    ViewData["RoleId"] = member.RoleId;
                    ViewData["Account"] = member;
                    if (member.Password != request.oldPass)
                    {
                        ViewData["ErrorOld"] = "The old password is incorrect!!!";
                        return View("Index");
                    }
                    else if (request.newPass != request.reNewPass)
                    {
                        ViewData["ErrorNew"] = "The new passwords don't match!!!";
                        return View("Index");
                    }
                    else
                    {
                        RestClient client = new RestClient(ApiPort);
                        var requesrUrl = new RestRequest($"api/Accounts/" + member.AccountId, RestSharp.Method.Put);
                        requesrUrl.AddHeader("content-type", "application/json");
                        var body = new Account
                        {
                            Email = member.Email,
                            Password = request.newPass,
                            Fullname = member.Fullname,
                            Avatar = member.Avatar,
                            Dob = member.Dob,
                            Gender = member.Gender,
                            Phone = member.Phone,
                            Address = member.Address,
                            RoleId = member.RoleId,
                        };
                        requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                        var response = await client.ExecuteAsync(requesrUrl);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("Index", "Profile");

        }
    }
}
