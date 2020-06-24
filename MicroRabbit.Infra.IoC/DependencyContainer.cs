using Microsoft.Extensions.DependencyInjection;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Application.Interfaces;
using MediatR;

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
            // Data
            //serviceCollection.AddTransient<BankingContext>();
            // Repo
            serviceCollection.AddTransient<IAccountRepository, AccountRepository>();
            // Services
            serviceCollection.AddTransient<IAccountService, AccountService>();
        }
    }
}
