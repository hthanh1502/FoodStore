using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Products
{
    public interface IProductRepository
    {
        void CreateProduct(ProductDTO p);
        Product GetProductById(int id);
        void DeleteProduct(Product p);
        void UpdateProduct(Product p);
        List<Product> GetAllProducts();
        ProductResponseDTO RequestProduct(ProductRequestDTO request);
        void Checkout(List<CartItem> list);
       
    }
}
