using System;
using System.Collections.Generic;
using MicroRabbit.Tranfer.Domain.Models;

namespace MicroRabbit.Tranfer.Domain.Interfaces
{
    public interface ITransferRepository
    {
        IEnumerable<AccountTransfer> GetAllTransfers();

        AccountTransfer GetSpecificTransfer(int transferId);
    }
}
