using Microsoft.AspNetCore.Mvc;

namespace FoodStoreWebClient.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Remove("loginUser");
            return RedirectToAction("Index", "Home");
        }
    }
}
