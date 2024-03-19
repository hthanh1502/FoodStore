using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Orders
{
    public interface IOrderRepositoty
    {
        void CreateOrder(Order o);
        Order GetOrderById(int id);
        List<Order> GetAllOrder();
        List<Order> AdminFillOrder(string startDate, string endDate);
        List<Order> GetOrderByAccountId(int id);
    }
}
