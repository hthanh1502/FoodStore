using BusinessObjects.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Orders
{
    public class OrderRepository : IOrderRepositoty
    {
        public List<Order> AdminFillOrder(string startDate, string endDate)
        {
            return OrderDAO.AdminFillOrder(startDate, endDate);
        }

        public void CreateOrder(Order o)
        {
            OrderDAO.CreateOrder(o);
        }

        public List<Order> GetAllOrder()
        {
            return OrderDAO.GetAllOrder();
        }

        public List<Order> GetOrderByAccountId(int id)
        {
            return OrderDAO.GetOrderByAccountId(id);
        }

        public Order GetOrderById(int id)
        {
            return OrderDAO.GetOrderById(id);
        }
    }
}
