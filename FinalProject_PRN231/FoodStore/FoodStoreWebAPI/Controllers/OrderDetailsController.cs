using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using DataAccess.Repository.OrderDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private IOrderDetailsRepository _repo;

        public OrderDetailsController(IOrderDetailsRepository repo)
        {
            _repo = repo;
        }

        // GET: api/OrderDetails
        [HttpGet]
        public ActionResult<IEnumerable<OrderDetail>> GetAllOrderDetails()
        {
            if (_repo.GetAllOrderDetails().Count == 0)
            {
                return NotFound();
            }
            return _repo.GetAllOrderDetails();
        }

        // GET: api/OrderDetails/5
        [HttpGet("{id}")]
        public ActionResult<List<OrderDetail>> GetOrderDetailByOrderId(int id)
        {
            if (_repo.GetAllOrderDetails().Count == 0)
            {
                return NotFound();
            }
            var listorderdetail = _repo.GetOrderDetailByOrderId(id);

            if (listorderdetail == null)
            {
                return NotFound();
            }
            return listorderdetail;
        }


        // POST: api/OrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<OrderDetail> CreateOrderDetail(OrderDetailDTO orderDetail)
        {
            var od = new OrderDetail
            {
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity,
                Price = orderDetail.Price,
                Discount = orderDetail.Discount
            };
            _repo.CreateOrderDetail(od);
            return NoContent();
        }
    }
}
