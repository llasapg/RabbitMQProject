using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Linq;
using RabbitMQ.Client.Events;
using System.Diagnostics;

namespace MicroRabbit.Infra.Bus
{
    /// <summary>
    /// Basic class for rabbit mq event bus
    /// </summary>
    public sealed class RabbitMQBus : IEventBus
    {
        #region Helpers
        public RabbitMQBus(IMediator mediator)
        {
            _mediator = mediator;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
        }
        /// Related to the commands
        private readonly IMediator _mediator;
        /// <summary>
        /// Collection for handlers only
        /// </summary>
        private readonly Dictionary<string ,List<Type>> _handlers; //string - event, List - handlers
        /// <summary>
        /// Collection for events only
        /// </summary>
        private readonly List<Type> _eventTypes;
        #endregion

        /// <summary>
        /// Async request to a single handler
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task SendCommand<T>(T command) where T : Command
        {
            await _mediator.Send(command); // how it can identify where this command shoud be send
        }

        /// <summary>
        /// Method to perform subscribing ( mapping between event and handler )
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name; // will get full event name
            var handler = typeof(TH);

            if(!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if(!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if(_handlers[eventName].Any(x => x.GetType() == handler))
            {
                throw new ArgumentException("Handler type name is already registered for event name");
            }

            _handlers[eventName].Add(handler); // adding handler for the given event

            BasicConsume<T>();
        }

        /// <summary>
        /// This method is used for publishing events to the event bus ( RabbitMq server )
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() {
                HostName = "localhost",
                Port = 5672,
                UserName = "llasapg",
                Password = "Gavras123321",
                ContinuationTimeout = TimeSpan.FromSeconds(60)
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    // we use event name to create or to use same named queue
                    var eventName = @event.GetType().Name;

                    channel.QueueDeclare(eventName, false, false, false, null); // !!!

                    var message = JsonConvert.SerializeObject(@event);

                    var body = Encoding.UTF8.GetBytes(message.ToString());

                    channel.BasicPublish("", eventName, null, body);
                }
            }
        }

        /// <summary>
        /// Basic consumer for rabbitMQ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void BasicConsume<T>() where T : Event
        {
            // connect to the rabbitMQ server
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "llasapg",
                Password = "Gavras123321",
                ContinuationTimeout = TimeSpan.FromSeconds(60),
                DispatchConsumersAsync = true
            };

            var queueName = typeof(T).Name; // get name of the queue where message will be stored

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queueName);

                    // Use little bit different consumer for this purpose
                    var consumer = new AsyncEventingBasicConsumer(channel);

                    consumer.Received += (sender, e) => ConsumerReceived(sender, e);

                    var result = consumer.Model.BasicConsume(queueName, true, consumer);
                }
            }
        }

        /// <summary>
        /// This method is used to handle any event in case of consiming from our queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task ConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey; // eventName

            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false); // Dont matter wheser to use another task, or be executed in this context
            }
            catch(Exception ex)
            {
                Trace.WriteLine($"Error occured - {ex.Message}");
            }
        }

        /// <summary>
        /// This method is used to process event that was consumed from the queue
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessEvent(string eventName, string message) // we pass eventName ( that can we use to get the handler name and message for the future process )
        {
            if(!_handlers.ContainsKey(eventName)) // we check that we have this type of events and handlers for them
            {
                var handlers = _handlers[eventName];

                foreach (var item in handlers)
                {
                    var handler = Activator.CreateInstance(item); // this class is used for object creation ( Actiovator )

                    if (handler == null)
                    {
                        var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);

                        var @event = JsonConvert.DeserializeObject(message, eventType); // in this step we are creating object, that represents our event ( Deserealize to the given type of event )

                        var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType); // we use this approach to get handler type with genetic

                        await (Task)handlerType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                    }
                }
            }
        }
    }
}
