using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.DTO
{
    public class ProductRequestDTO
    {
        public string? search { get; set; }
        public int categoryId { get; set; }
        public int sort { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
    }
}
