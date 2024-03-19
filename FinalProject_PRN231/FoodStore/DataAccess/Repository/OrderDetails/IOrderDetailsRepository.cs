using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.OrderDetails
{
    public interface IOrderDetailsRepository
    {
        void CreateOrderDetail(OrderDetail od);
        List<OrderDetail> GetOrderDetailByOrderId(int id);
        List<OrderDetail> GetAllOrderDetails();
    }
}
