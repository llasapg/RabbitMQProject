using System;
using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Domain.Core.Commands
{
    /// <summary>
    /// Core class for our commands
    /// </summary>
    public abstract class Command : Message // create account command
    {
        /// <summary>
        /// Timestamp to know time when this command was send
        /// </summary>
        public DateTime Timestamp { get; protected set; }

        protected Command() => Timestamp = DateTime.Now;
    }
}
