using BankAccount_Form.Data;
using BankAccount_Form.DTO;
using BankAccount_Form.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankAccount_Form.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BankController : ControllerBase
    {
        private readonly BankDbContext bankDb;

        private readonly IConfiguration configuration;

        public BankController(BankDbContext bankDb, IConfiguration configuration)
        {
            this.bankDb = bankDb;
            this.configuration = configuration;
        }
        

        [HttpGet]

        public IActionResult Get()
        {
            List<BankAccount> bankAccounts = bankDb.BankAccounts.ToList();
            return Ok(bankAccounts);
        }

        [HttpGet]
        [Route("{Id:int}")]

        public IActionResult GetId(int Id)
        {
            BankAccount bank = bankDb.BankAccounts.FirstOrDefault(x => x.Id == Id);
            return Ok(bank);
        }

        [HttpPost]

        public IActionResult Insert([FromBody] BankAccount bankAccount)
        {
            bankDb.BankAccounts.Add(bankAccount);
            bankDb.SaveChanges();
            return Ok(bankAccount);
        }

        [HttpPut]
        [Route("{Id:int}")]

        public IActionResult Update(int Id, [FromBody] BankAccount bankAccount)
        {
            BankAccount bankAccount1 = bankDb.BankAccounts.FirstOrDefault(y => y.Id == Id);
            bankAccount1.FullName = bankAccount.FullName;
            bankAccount1.Email = bankAccount.Email;
            bankAccount1.Password = bankAccount.Password;
            bankAccount1.PhoneNo = bankAccount.PhoneNo;
            bankAccount1.Address = bankAccount.Address;
            bankAccount1.Account = bankAccount.Account;
            bankAccount1.DebitCard = bankAccount.DebitCard;
            bankAccount1.InternetBanking = bankAccount.InternetBanking;
            bankAccount1.MobileAlert = bankAccount.MobileAlert;
            bankAccount1.Selectbranch = bankAccount.Selectbranch;
            bankDb.SaveChanges();
            return Ok(bankAccount);
        }

        [HttpDelete]

        [Route("{Id:int}")]

        public IActionResult Delete(int Id)
        {
            BankAccount bankAccount = bankDb.BankAccounts.FirstOrDefault(x => x.Id == Id);
            bankDb.BankAccounts.Remove(bankAccount);
            bankDb.SaveChanges();
            return Ok(bankAccount);
        }


        [AllowAnonymous]
        [HttpGet("{email}/{password}")]
        public IActionResult Login(string email, string password)
        {
            var result = new Login();
            var user = bankDb.BankAccounts.FirstOrDefault(p => p.Email == email && p.Password == password);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                result.UserName = user.FullName;
                var claims = new[]
                {
                  new Claim(JwtRegisteredClaimNames.Sub,email),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(ClaimTypes.Name, email)

                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    configuration["Jwt:Issuer"],
                    configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds
                );
                result.Token = new JwtSecurityTokenHandler().WriteToken(token);
            }
            return Ok(result);

        }
    }
}