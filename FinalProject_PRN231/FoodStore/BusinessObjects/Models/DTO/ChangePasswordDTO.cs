using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.DTO
{
    public class ChangePasswordDTO
    {
        public string oldPass { get; set; }
        public string newPass { get; set; }
        public string reNewPass { get; set; }
    }
}
