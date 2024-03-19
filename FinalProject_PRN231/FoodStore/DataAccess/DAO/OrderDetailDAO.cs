using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class OrderDetailDAO
    {
        public static void CreateOrderDetail(OrderDetail od)
        {
            try
            {
                using (var context = new FoodStoreContext())
                {
                    OrderDetail orderdetail = new OrderDetail
                    {
                        OrderId = od.OrderId,
                        ProductId = od.ProductId,
                        Quantity = od.Quantity,
                        Price = od.Price,
                        Discount = od.Discount,
                    };
                    context.OrderDetails.Add(orderdetail);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<OrderDetail> GetAllOrderDetails()
        {
            var listOrderDetails = new List<OrderDetail>();
            try
            {
                using (var context = new FoodStoreContext())
                {
                    listOrderDetails = context.OrderDetails.ToList();
                }
                foreach (var item in listOrderDetails)
                {
                    item.Product = ProductDAO.GetProductById(item.ProductId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listOrderDetails;
        }

        public static List<OrderDetail> GetOrderDetailByOrderId(int id)
        {
            List<OrderDetail> list = new List<OrderDetail>();
            try
            {
                using (var context = new FoodStoreContext())
                {
                    list = context.OrderDetails.Where(x => x.OrderId == id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }
    }
}
