using BankAccount_Form.Entity;
using Microsoft.EntityFrameworkCore;

namespace BankAccount_Form.Data
{
    public class BankDbContext:DbContext
    {

        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options) 
        { 
        }

        public DbSet<BankAccount> BankAccounts { get; set; }
    }
}
