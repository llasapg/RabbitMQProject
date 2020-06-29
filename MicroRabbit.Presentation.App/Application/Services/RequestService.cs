using System;
using System.Net.Http;
using System.Threading.Tasks;
using MicroRabbit.Presentation.App.Application.Interfaces;

namespace MicroRabbit.Presentation.App.Application.Services
{
    public class RequestService : IRequestService
    {
        private readonly HttpClient _client;

        public RequestService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> SendRequest(string uri, HttpContent message)
        {
            var response = await _client.PostAsync(uri, message);

            return response.Content.ToString();
        }
    }
}
