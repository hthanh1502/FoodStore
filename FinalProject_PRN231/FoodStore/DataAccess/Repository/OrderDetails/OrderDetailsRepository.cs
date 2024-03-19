using BusinessObjects.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.OrderDetails
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        public void CreateOrderDetail(OrderDetail od)
        {
            OrderDetailDAO.CreateOrderDetail(od);
        }

        public List<OrderDetail> GetAllOrderDetails()
        {
            return OrderDetailDAO.GetAllOrderDetails();
        }

        public List<OrderDetail> GetOrderDetailByOrderId(int id)
        {
            return OrderDetailDAO.GetOrderDetailByOrderId(id);
        }
    }
}
