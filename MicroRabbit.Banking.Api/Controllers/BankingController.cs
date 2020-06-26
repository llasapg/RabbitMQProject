using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Data;
using MicroRabbit.Domain.Core.Bus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroRabbit.Banking.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankingController : ControllerBase
    {
        private readonly ILogger<BankingController> _logger;
        private readonly IAccountService _accountService;

        public BankingController(ILogger<BankingController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet("GetAllAccounts")]
        public IActionResult GetAllAccounts()
        {
            _logger.LogInformation("Accounts requested");

            return Ok(new { result = _accountService.GetAccounts()});
        }

        [HttpPost("CreateTransaction")]
        public IActionResult CreateTransaction([FromBody] ChangeBalanceTransaction transaction)
        {
            _logger.LogInformation($"Transaction received");

            // send command to create transfer

            _accountService.CreateTransfer(new ChangeBalanceTransaction {Amount = transaction.Amount, FromAccount = transaction.FromAccount, ToAccount = transaction.ToAccount });

            return Ok(null);
        }
    }
}
