using System;

namespace MicroRabbit.Banking.Application.Models
{
    public abstract class Transaction
    {
        protected int TransactionId { get; set; }
        protected DateTime TransactionTime { get; set; }

        protected Transaction()
        {
            TransactionTime = DateTime.Now;
        }
    }
}
