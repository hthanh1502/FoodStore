using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class ProductDAO
    {
        public static List<Product> GetAllProducts()
        {
            var listProducts = new List<Product>();
            try
            {
                using (var context = new FoodStoreContext())
                {
                    listProducts = context.Products.ToList();
                }
                foreach (var item in listProducts)
                {
                    item.Category = CategoryDAO.GetCategoryById(item.CategoryId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProducts;
        }

        public static Product GetProductById(int prodId)
        {
            Product p = new Product();
            try
            {
                using (var context = new FoodStoreContext())
                {
                    p = context.Products.SingleOrDefault(x => x.ProductId == prodId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return p;
        }
        public static void CreateProduct(ProductDTO p)
        {
            try
            {
                using (var context = new FoodStoreContext())
                {
                    Product product = new Product
                    {
                        ProductName = p.ProductName,
                        ProductImage = p.ProductImage,
                        ProductDescription = p.ProductDescription,
                        CategoryId = p.CategoryId,
                        Price = p.Price,
                        Discount = p.Discount,
                        Quantity = p.Quantity,
                        Active = p.Active,
                    };
                    context.Products.Add(product);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateProduct(Product p)
        {
            try
            {
                using (var context = new FoodStoreContext())
                {
                    context.Entry<Product>(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteProduct(Product product)
        {
            try
            {
                using (var context = new FoodStoreContext())
                {
                    var p1 = context.Products.SingleOrDefault(c => c.ProductId == product.ProductId);
                    // p1.Active = false;
                    context.Products.Remove(p1);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static ProductResponseDTO RequestProduct(ProductRequestDTO request)
        {
            ProductResponseDTO response = new ProductResponseDTO();
            var context = new FoodStoreContext();
            try
            {
                List<Product> listProducts = context.Products.Where(x => x.Active == true).ToList();
                if (request.search.Trim() != null || request.search.Trim() != "")
                {
                    if (request.search.Equals("all"))
                    {
                        listProducts = listProducts;
                    }
                    else
                    {
                        listProducts = listProducts.Where(x => x.ProductName.ToLower().Contains(request.search.ToLower())).ToList();

                    }
                }
                if (request.categoryId != 0)
                {
                    listProducts = listProducts.Where(x => x.CategoryId == request.categoryId).ToList();
                }
                if (request.sort != 0)
                {
                    if (request.sort == 1)
                    {
                        listProducts = listProducts.OrderBy(x => x.Price).ToList();
                    }
                    if (request.sort == 2)
                    {
                        listProducts = listProducts.OrderByDescending(x => x.Price).ToList();
                    }
                }
                int totalItems = listProducts.Count();
                int totalPages = (int)Math.Ceiling((double)totalItems / request.pageSize);
                listProducts = listProducts.Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize).ToList();
                foreach (var item in listProducts)
                {
                    item.Category = CategoryDAO.GetCategoryById(item.CategoryId);
                }
                response.items = listProducts;
                response.totalPages = totalPages;
                response.pageIndex = request.pageIndex;
                response.pageSize = request.pageSize;
                if (request.pageIndex == totalPages)
                {
                    response.last = true;
                }
                else
                {
                    response.last = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }

        public static void Checkout(List<CartItem> cartItems)
        {
            try
            {
                using (var context = new FoodStoreContext())
                {
                    foreach (var item in cartItems)
                    {
                        Product p = context.Products.Where(x => x.ProductId == item.product.ProductId).FirstOrDefault();
                        int quantity = p.Quantity - item.quantity;
                        p.Quantity = quantity;
                        context.Entry<Product>(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
