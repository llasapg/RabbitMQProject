using System;
using System.Collections.Generic;
using System.Transactions;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Application.Services
{
    public class AccountService : IAccountService
    {
        private IAccountRepository AccountRepository { get; }
        private readonly IEventBus _bus;

        public AccountService(IAccountRepository accountRepository, IEventBus bus)
        {
            AccountRepository = accountRepository;
            _bus = bus;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return AccountRepository.GetAllAccounts();
        }

        public async void CreateTransfer(ChangeBalanceTransaction transaction)
        {
            // create command
            var command = new CreateTransferCommand() {Amount = transaction.Amount, FromAccount = transaction.FromAccount, ToAccount = transaction.ToAccount };
            // publish it to the event bus
            await _bus.SendCommand(command); // ? if i didnt use an await we can have an error in dif thred and it will not be displayed
        }
    }
}
