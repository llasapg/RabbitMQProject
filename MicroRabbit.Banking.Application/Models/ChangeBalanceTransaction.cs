using System;
namespace MicroRabbit.Banking.Application.Models
{
    public class ChangeBalanceTransaction : Transaction
    {
        public ChangeBalanceTransaction(Guid fromBalance, Guid ToBalance, decimal amount)
        {
            FromAccount = fromBalance;
            ToAccount = ToBalance;
            Amount = amount;
        }

        public ChangeBalanceTransaction()
        {

        }

        public Guid FromAccount { get; set; }
        public Guid ToAccount { get; set; }
        public decimal Amount { get; set; }
    }
}
