using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.DTO
{
    public class ProductDTO
    {
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public double Discount { get; set; }
        public int Quantity { get; set; }
        public bool Active { get; set; }
    }
}
