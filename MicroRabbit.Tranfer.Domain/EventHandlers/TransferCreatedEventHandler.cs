using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Tranfer.Domain.Events;
using MediatR;

namespace MicroRabbit.Tranfer.Domain.EventHandlers
{
    public class TransferCreatedEventHandler : IEventHandler<TransferCreatedEvent>
    {
        public Task Handle(TransferCreatedEvent @event)
        {
            Trace.WriteLine("Event handled");

            return Task.CompletedTask;
        }
    }
}
