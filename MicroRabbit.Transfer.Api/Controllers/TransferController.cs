using System.Collections.Generic;
using System.Linq;
using MicroRabbit.Tranfer.Domain.Models;
using MicroRabbit.Transfer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroRabbit.Transfer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly ILogger<TransferController> _logger;
        private readonly ITransferService _service;

        public TransferController(ILogger<TransferController> logger, ITransferService transferService)
        {
            _logger = logger;
            _service = transferService;
        }

        [HttpGet("GetAllTransfers")]
        public IEnumerable<AccountTransfer> GetAllTransfers()
        {
            var result = _service.GetAllTransfers();

            _logger.LogInformation($"Transfers returned, total count - {result.Count()}");

            return result.ToList();
        }
    }
}
