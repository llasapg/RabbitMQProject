using System;
using System.Collections.Generic;
using MicroRabbit.Tranfer.Domain.Models;

namespace MicroRabbit.Transfer.Application.Interfaces
{
    public interface ITransferService
    {
        IEnumerable<AccountTransfer> GetAllTransfers();

        AccountTransfer GetSpecificTransfer(int transferId);
    }
}
