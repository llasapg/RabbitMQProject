using System;
using System.Collections.Generic;
using System.Linq;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Data.Repository
{
    /// <summary>
    /// Class with methods for accessing DB
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        private  BankingContext Context {get;}

        public AccountRepository(BankingContext bankingContext)
        {
            Context = bankingContext;
        }

        /// <summary>
        /// Returns all the accounts stored in DB
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Account> GetAllAccounts()
        {
            return Context.Accounts.ToList();
        }
    }
}
