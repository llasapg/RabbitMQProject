using System;
namespace MicroRabbit.Tranfer.Domain.Models
{
    public class AccountTransfer
    {
        public int Id { get; set; }
        public Guid FromAccount { get; set; }
        public Guid ToAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransferTime { get; set; }
    }
}
