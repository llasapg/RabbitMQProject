using Microsoft.EntityFrameworkCore;
using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Data
{
    public class BankingContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public BankingContext(DbContextOptions<BankingContext> options) : base(options)
        {
            Database.Migrate();
        }
    }
}
