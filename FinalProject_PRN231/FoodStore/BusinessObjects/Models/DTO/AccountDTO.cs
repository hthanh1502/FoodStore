using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.DTO
{
    public class AccountDTO
    {
        public int AccountId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Fullname { get; set; } = null!;
        public string? Avatar { get; set; }
        public DateTime? Dob { get; set; }
        public bool? Gender { get; set; }
        public string Phone { get; set; } = null!;
        public string? Address { get; set; }
        public int RoleId { get; set; }
    }
}
