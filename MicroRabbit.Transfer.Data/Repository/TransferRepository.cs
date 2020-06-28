using System;
using System.Collections.Generic;
using System.Linq;
using MicroRabbit.Tranfer.Domain.Interfaces;
using MicroRabbit.Tranfer.Domain.Models;
using MicroRabbit.Transfer.Data.Context;

namespace MicroRabbit.Transfer.Data.Repository
{
    public class TransferRepository : ITransferRepository
    {
        private readonly TransferDbContext _context;

        public TransferRepository(TransferDbContext transferDbContext)
        {
            _context = transferDbContext;
        }

        public IEnumerable<AccountTransfer> GetAllTransfers()
        {
            return _context.Transfers.ToList();
        }

        public AccountTransfer GetSpecificTransfer(int transferId)
        {
            return _context.Transfers.ToList().Where(x => x.Id == transferId).First();
        }
    }
}
