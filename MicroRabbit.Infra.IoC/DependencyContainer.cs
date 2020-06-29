using Microsoft.Extensions.DependencyInjection;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Tranfer.Domain.Interfaces;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Data.Repository;
using MicroRabbit.Transfer.Application.Services;
using MediatR;
using MicroRabbit.Tranfer.Domain.EventHandlers;

namespace MicroRabbit.Infra.IoC
{
    /// <summary>
    /// Simple class to perform dependency-injection
    /// </summary>
    public class DependencyContainer
    {
        public static void RegisterServicesForBanking(IServiceCollection serviceCollection) // currently we use Microsoft DepInjection container for our needs
        {
            // Domain Bus ( EventBus realm )
            serviceCollection.AddSingleton<IEventBus, RabbitMQBus>();
            // Event handlers
            // Repo
            serviceCollection.AddTransient<IAccountRepository, AccountRepository>();
            // Services
            serviceCollection.AddTransient<IAccountService, AccountService>();
        }

        public static void RegisterServicesForTransfer(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IEventBus, RabbitMQBus>();

            serviceCollection.AddTransient<TransferCreatedEventHandler>();

            serviceCollection.AddTransient<ITransferRepository, TransferRepository>();

            serviceCollection.AddTransient<ITransferService, TransferService>();

            // Event handlers
        }

        public static void RegisterServicesForPresentation(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IEventBus, RabbitMQBus>();
        }
    }
}
