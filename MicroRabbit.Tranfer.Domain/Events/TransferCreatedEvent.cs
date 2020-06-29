using System;
using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Tranfer.Domain.Events
{
    public class TransferCreatedEvent : Event
    {
        public Guid FromAccount { get; set; }
        public Guid ToAccount { get; set; }
        public decimal Amount { get; set; }

        public TransferCreatedEvent(Guid from, Guid to, decimal amount)
        {
            FromAccount = from;
            ToAccount = to;
            Amount = amount;
        }
    }
}
