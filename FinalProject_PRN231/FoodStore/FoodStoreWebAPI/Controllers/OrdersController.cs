using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using DataAccess.Repository.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IOrderRepositoty _repo;

        public OrdersController(IOrderRepositoty repo)
        {
            _repo = repo;
        }

        // GET: api/Orders
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetAllOrders()
        {
            if (_repo.GetAllOrder().Count == 0)
            {
                return NotFound();
            }
            return _repo.GetAllOrder();
        }
        [HttpGet("Admin/{startDate}/{endDate}")]
        public ActionResult<IEnumerable<Order>> AdminFillOrder(string startDate, string endDate)
        {
            if (_repo.GetAllOrder().Count == 0)
            {
                return NotFound();
            }
            if (startDate == null || endDate == null) { return _repo.GetAllOrder(); }

            return _repo.AdminFillOrder(startDate, endDate);

        }

        // GET: api/Orders/5
        [HttpGet("GetById/{id}")]
        public ActionResult<Order> GetOrderById(int id)
        {
            if (_repo.GetAllOrder().Count == 0)
            {
                return NotFound();
            }
            var order = _repo.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        // GET: api/Orders/5
        [HttpGet("GetByAccountId/{id}")]
        public ActionResult<List<Order>> GetOrderByAccountId(int id)
        {
            if (_repo.GetAllOrder().Count == 0)
            {
                return NotFound();
            }
            var order = _repo.GetOrderByAccountId(id);

            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Order> CreateOrder(OrderDTO order)
        {
            var or = new Order
            {
                AccountId = order.AccountId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount
            };
            _repo.CreateOrder(or);
            return Ok(or);
        }
    }
}
