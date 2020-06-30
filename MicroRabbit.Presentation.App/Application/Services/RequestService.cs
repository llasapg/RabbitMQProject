using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

        public async Task<string> SendRequest(string uri, string message)
        {
            var content = new HttpRequestMessage(HttpMethod.Post, uri);

            content.Content = new StringContent(message, Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.SendAsync(content);

            return response.Content.ToString();
        }
    }
}
