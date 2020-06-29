using System.Net.Http;
using System.Threading.Tasks;

namespace MicroRabbit.Presentation.App.Application.Interfaces
{
    public interface IRequestService
    {
        Task<string> SendRequest(string uri, HttpContent message);
    }
}
