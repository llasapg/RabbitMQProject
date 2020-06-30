using System;
namespace MicroRabbit.Presentation.App.Domain.Models
{
    public class TransferViewModel
    {
        public Guid FormAccount { get; set; }
        public Guid ToAccount { get; set; }
        public decimal Amount { get; set; }
    }
}
