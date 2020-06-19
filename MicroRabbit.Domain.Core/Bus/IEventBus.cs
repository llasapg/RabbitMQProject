using System;
using System.Threading.Tasks;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Domain.Core.Bus
{
    /// <summary>
    /// In future we can have lots of different event buses, so its good idea to have an abstraction
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// This method is used for sending command to the bus
        /// </summary>
        /// <param name="command"></param>        
        Task SendCommand<T>(T command) where T : Command;

        /// <summary>
        /// This methos is used for sending events to our bus
        /// </summary>
        /// <param name="event"></param>
        void Publish<T>(T @event) where T : Event;

        /// <summary>
        /// Basic method to perform subscribing for some sort of event
        /// </summary>
        void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;
    }
}
