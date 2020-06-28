using System.Collections.Generic;
using MicroRabbit.Tranfer.Domain.Interfaces;
using MicroRabbit.Tranfer.Domain.Models;
using MicroRabbit.Transfer.Application.Interfaces;

namespace MicroRabbit.Transfer.Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _repo;

        public TransferService(ITransferRepository transferRepository)
        {
            _repo = transferRepository;
        }

        public IEnumerable<AccountTransfer> GetAllTransfers()
        {
            return _repo.GetAllTransfers();
        }

        public AccountTransfer GetSpecificTransfer(int transferId)
        {
            return _repo.GetSpecificTransfer(transferId);
        }
    }
}
