using System;
using Microsoft.EntityFrameworkCore;
using MicroRabbit.Tranfer.Domain.Models;

namespace MicroRabbit.Transfer.Data.Context
{
    public class TransferDbContext: DbContext
    {
        public DbSet<AccountTransfer> Transfers { get; set; }

        public TransferDbContext(DbContextOptions<TransferDbContext> dbContextOptions) : base(dbContextOptions)
        {
            Database.Migrate();
        }
    }
}
