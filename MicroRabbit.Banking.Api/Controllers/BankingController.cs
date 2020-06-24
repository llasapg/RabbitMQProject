using MicroRabbit.Banking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroRabbit.Banking.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankingController : ControllerBase
    {
        private readonly ILogger<BankingController> _logger;
        private readonly BankingContext _bankingContext;

        public BankingController(ILogger<BankingController> logger, BankingContext bankingContext)
        {
            _logger = logger;
            _bankingContext = bankingContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Accounts requested");

            return Ok(new { result = _bankingContext.Accounts });
        }
    }
}
