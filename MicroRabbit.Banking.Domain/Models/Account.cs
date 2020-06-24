using System;
namespace MicroRabbit.Banking.Domain.Models
{
    /// <summary>
    /// Basic model to work with customer account
    /// </summary>
    public class Account
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public Guid GuidAccountId { get; set; }
        public string AccountType { get; set; }
        public decimal AccountBalance { get; set; }
    }
}
