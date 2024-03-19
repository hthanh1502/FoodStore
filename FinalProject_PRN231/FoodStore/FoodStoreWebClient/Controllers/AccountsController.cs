using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Headers;

namespace FoodStoreWebClient.Controllers
{
    public class AccountsController : Controller
    {
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private FoodStoreContext context;

        public AccountsController(IConfiguration configuration, FoodStoreContext context)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
            this.context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/accounts");
            string strData = await response.Content.ReadAsStringAsync();
            List<Account> listAccounts = JsonConvert.DeserializeObject<List<Account>>(strData);
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }
            ViewData["Role"] = new SelectList(context.Roles, "RoleId", "RoleName");
            return View(listAccounts);
        }

        public async Task<List<Role>> GetAllRoles()
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/Roles");
            string strData = await response.Content.ReadAsStringAsync();
            List<Role> listRoles = JsonConvert.DeserializeObject<List<Role>>(strData);
            return listRoles;
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/Accounts/byId/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            Account account = JsonConvert.DeserializeObject<Account>(strData);
            if (id == null || account == null)
            {
                return NotFound();
            }
            ViewData["RoleName"] = GetAllRoles().Result.Where(x => x.RoleId == account.RoleId).FirstOrDefault().RoleName;

            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }
            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            ViewData["Role"] = new SelectList(context.Roles, "RoleId", "RoleName");
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }
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
            Account account = JsonConvert.DeserializeObject<Account>(strData);
            return account;
        }
        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Email,Password,Fullname,Avatar,Dob,Gender,Phone,Address,RoleId")] Account account, IFormFile avtFile)
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
                    var requesrUrl = new RestRequest($"api/Accounts", RestSharp.Method.Post);
                    requesrUrl.AddHeader("content-type", "application/json");
                    var body = new Account
                    {
                        Email = account.Email,
                        Password = account.Password,
                        Fullname = account.Fullname,
                        Avatar = account.Avatar,
                        Dob = account.Dob,
                        Gender = account.Gender,
                        Phone = account.Phone,
                        Address = account.Address,
                        RoleId = account.RoleId
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
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/Accounts/byId/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var account = JsonConvert.DeserializeObject<Account>(strData);
            if (account == null)
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
            ViewData["Role"] = new SelectList(context.Roles, "RoleId", "RoleName");
            return View(account);
        }

        //POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Email,Password,Fullname,Avatar,Dob,Gender,Phone,Address,RoleId")] AccountDTO account, IFormFile avtFile)
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
                var requesrUrl = new RestRequest($"api/Accounts/{id}", RestSharp.Method.Put);
                requesrUrl.AddHeader("content-type", "application/json");
                var body = new AccountDTO
                {
                    Email = account.Email,
                    Password = account.Password,
                    Fullname = account.Fullname,
                    Avatar = account.Avatar,
                    Dob = account.Dob,
                    Gender = account.Gender,
                    Phone = account.Phone,
                    Address = account.Address,
                    RoleId = account.RoleId
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
    }
}
