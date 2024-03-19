using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.DTO
{
    public class CartItem
    {
        public int quantity { set; get; }
        public Product product { set; get; }
    }
}
