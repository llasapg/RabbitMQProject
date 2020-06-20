using Microsoft.Extensions.DependencyInjection;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;

namespace MicroRabbit.Infra.IoC
{
    /// <summary>
    /// Simple class to perform dependency-injection
    /// </summary>
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection serviceCollection)
        {
            // Domain Bus ( EventBus realm )
            serviceCollection.AddTransient<IEventBus, RabbitMQBus>();
        }
    }
}
