using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Net.Http.Headers;

namespace FoodStoreWebClient.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";


        public CategoriesController(IConfiguration configuration)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }

        //GET: Categories
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/categories");
            string strData = await response.Content.ReadAsStringAsync();
            List<Category> listCategories = JsonConvert.DeserializeObject<List<Category>>(strData);
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }
            return View(listCategories);
        }

        //GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/categories/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            Category category = JsonConvert.DeserializeObject<Category>(strData);
            if (category == null)
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
            return View(category);
        }

        //GET: Categories/Create
        public IActionResult Create()
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

        //POST: Categories/Create
        //To protect from overposting attacks, enable the specific properties you want to bind to.
        //For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] Category category)
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/categories", RestSharp.Method.Post);
                requesrUrl.AddHeader("content-type", "application/json");
                var cate = new Category
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName
                };
                requesrUrl.AddParameter("application/json-patch+json", cate, ParameterType.RequestBody);
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

        //GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/categories/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var cate = JsonConvert.DeserializeObject<Category>(strData);
            if (cate == null)
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
            return View(cate);
        }

        //POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName")] Category category)
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/categories/{id}", RestSharp.Method.Put);
                requesrUrl.AddHeader("content-type", "application/json");
                var cate = new Category
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName
                };
                requesrUrl.AddParameter("application/json-patch+json", cate, ParameterType.RequestBody);
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

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/categories/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            Category category = JsonConvert.DeserializeObject<Category>(strData);
            if (category == null)
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
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requestUrl = new RestRequest($"api/categories/{id}", Method.Delete);
                var response = await client.ExecuteAsync(requestUrl);

                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    // Handle non-successful status code (if needed)
                    return View("Error");
                }
            }
            catch (Exception)
            {
                // Handle exceptions appropriately (log, show error page, etc.)
                return View("Error");
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
