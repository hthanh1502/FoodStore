using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FoodStoreWebClient.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;

        private readonly FoodStoreContext _context;

        public CartController(ILogger<CartController> logger, FoodStoreContext context)
        {
            _logger = logger;
            _context = context;
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

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItem> list)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(list);
            session.SetString(CARTKEY, jsoncart);
        }

        // Hiện thị danh sách sản phẩm, có nút chọn đưa vào giỏ hàng
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        /// Thêm sản phẩm vào cart
        public IActionResult AddToCart(int productId)
        {
            var sessionUser = HttpContext.Session.GetString("loginUser");
            if (sessionUser != null)
            {
                var product = _context.Products
                            .Where(p => p.ProductId == productId)
                            .FirstOrDefault();
                if (product == null)
                    return NotFound("Không có sản phẩm");

                // Xử lý đưa vào Cart ...
                var cart = GetCartItems();
                var cartitem = cart.Find(p => p.product.ProductId == productId);
                if (cartitem != null)
                {
                    // Đã tồn tại, tăng thêm 1
                    cartitem.quantity++;
                }
                else
                {
                    //  Thêm mới
                    cart.Add(new CartItem() { quantity = 1, product = product });
                }
                // Lưu cart vào Session
                SaveCartSession(cart);
                // Chuyển đến trang hiện thị Cart
                return RedirectToAction(nameof(Cart));
            }
            return RedirectToAction("Index", "Login");
        }
        /// xóa item trong cart
        [Route("/removecart/{productid:int}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] int productid)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductId == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cart.Remove(cartitem);
            }

            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }

        /// Cập nhật
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromForm] int productid, [FromForm] int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductId == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity = quantity;
            }
            SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }


        // Hiện thị giỏ hàng
        public IActionResult Cart()
        {
            var cart = GetCartItems();
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                var member = JsonConvert.DeserializeObject<Account>(session);
                ViewData["RoleId"] = member.RoleId;
                ViewData["Account"] = member;
            }
            return View(GetCartItems());
        }
    }
}
