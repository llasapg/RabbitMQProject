using System;
using System.Threading.Tasks;
using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Domain.Core.Bus
{
    /// <summary>
    /// General insterface for handling events
    /// Can handle any event
    /// </summary>
    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent: Event
    {
        Task Handle(TEvent @event); // will be added
    }

    public interface IEventHandler
    {

    }
}
