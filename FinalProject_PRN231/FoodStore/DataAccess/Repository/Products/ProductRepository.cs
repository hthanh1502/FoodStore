using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Products
{
    public class ProductRepository : IProductRepository
    {
        public void DeleteProduct(Product p)
        {
            ProductDAO.DeleteProduct(p);
        }

        public List<Product> GetAllProducts()
        {
            return ProductDAO.GetAllProducts();
        }

        public Product GetProductById(int id)
        {
            return ProductDAO.GetProductById(id);
        }

        public void CreateProduct(ProductDTO p)
        {
            ProductDAO.CreateProduct(p);
        }

        public void UpdateProduct(Product p)
        {
            ProductDAO.UpdateProduct(p);
        }

        public ProductResponseDTO RequestProduct(ProductRequestDTO request)
        {
            return ProductDAO.RequestProduct(request);
        }

        public void Checkout(List<CartItem> list)
        {
            ProductDAO.Checkout(list);
        }

        
    }
}
