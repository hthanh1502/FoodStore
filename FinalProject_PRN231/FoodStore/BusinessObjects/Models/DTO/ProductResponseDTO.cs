using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.DTO
{
    public class ProductResponseDTO
    {
        public List<Product> items { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
        public int totalPages { get; set; }
        public bool last { get; set; }
    }
}
