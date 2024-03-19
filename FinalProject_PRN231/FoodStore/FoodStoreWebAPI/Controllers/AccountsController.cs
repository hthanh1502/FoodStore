using BusinessObjects.Models.DTO;
using BusinessObjects.Models;
using DataAccess.Repository.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountRepository _repo;

        public AccountsController(IAccountRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAccounts()
        {
            return _repo.GetAllAccount();
        }

        // GET: api/Accounts/5
        [HttpGet("byId/{id}")]
        public async Task<ActionResult<Account>> GetAccountById(int id)
        {
            var account = _repo.GetAccountById(id);

            if (account == null)
            {
                return NotFound();
            }
            return account;
        }


        [HttpGet("ByEmail/{email}")]
        public async Task<ActionResult<Account>> GetAccountByEmail(string email)
        {
            var account = _repo.GetAccountByEmail(email);

            if (account == null)
            {
                return NotFound();
            }
            return account;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult UpdateAccount(int id, AccountDTO accountDTO)
        {
            var acc = _repo.GetAccountById(id);
            if (acc == null)
            {
                return BadRequest();
            }
            acc.Fullname = accountDTO.Fullname;
            acc.Password = accountDTO.Password;
            acc.Avatar = accountDTO.Avatar;
            acc.Dob = accountDTO.Dob;
            acc.Gender = accountDTO.Gender;
            acc.Phone = accountDTO.Phone;
            acc.Address = accountDTO.Address;
            acc.RoleId = accountDTO.RoleId;
            _repo.UpdateAccount(acc);
            return NoContent();
        }

        //POST: api/Accounts
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Register")]
        public ActionResult<Account> Register(RegisterDTO request)
        {
            Account a = new Account
            {
                Email = request.Email,
                Password = request.Password,
                Fullname = request.Fullname,
                Avatar = request.Avatar,
                Dob = request.Dob,
                Gender = request.Gender,
                Phone = request.Phone,
                Address = request.Address,
                RoleId = 2
            };
            _repo.CreateAccount(a);
            return NoContent();
        }
        //POST: api/Accounts
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Account> PostAccount(AccountDTO request)
        {
            Account a = new Account
            {
                Email = request.Email,
                Password = request.Password,
                Fullname = request.Fullname,
                Avatar = request.Avatar,
                Dob = request.Dob,
                Gender = request.Gender,
                Phone = request.Phone,
                Address = request.Address,
                RoleId = request.RoleId
            };
            _repo.CreateAccount(a);
            return NoContent();
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Account>> Login(LoginRequest request)
        {
            var account = _repo.Login(request.Email, request.Password);
            if (account == null)
            {
                return NotFound();
            }
            return account;
        }
    }
}
