using System;
using System.Net.Http;
using System.Threading.Tasks;
using MicroRabbit.Presentation.App.Application.Interfaces;
using MicroRabbit.Presentation.App.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MicroRabbit.Presentation.App.Controllers
{
    public class HomePageController : Controller
    {
        private readonly IRequestService _requestService;

        public HomePageController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransfer(TransferViewModel viewModel)
        {
            var model = JsonConvert.SerializeObject(viewModel);

            await _requestService.SendRequest("https://localhost:9000/Banking/CreateTransaction", model);

            return View("Index");
        }
    }
}
