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

namespace MicroRabbit.Infra.Bus
{
    /// <summary>
    /// Basic class for rabbit mq event bus
    /// </summary>
    public sealed class RabbitMQBus : IEventBus
    {
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

        /// Related to the events

        public void Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>
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

            // StartBasicConsume<T>();
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

                    channel.QueueDeclare(eventName); // !!!

                    var message = JsonConvert.SerializeObject(@event);

                    var body = Encoding.UTF8.GetBytes(message.ToString());

                    channel.BasicPublish("", eventName, false, null, body);
                }
            }
        }

        /// <summary>
        /// Basic consumer for rabbitMQ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private async void BasicConsume<T>() where T : Event
        {
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

                    var consumer = new AsyncEventingBasicConsumer(channel);

                    consumer.Received += ConsumerReceived()

                    var result = consumer.Model.BasicConsume(queueName, true, consumer);

                    //get the message body

                    var messageBody = Encoding.UTF8.GetString(result);
                }
            }
        }

        //public delegate void ConsumerReceived(object sender, BasicDeliverEventArgs e);

        private async Task ConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey; // eventName

            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            // event process

            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch(Exception ex)
            {

            }
        }

        /// <summary>
        /// todo - Check this stuff!!!!
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessEvent(string eventName, string message) // cool
        {
            if(!_handlers.ContainsKey(eventName))
            {
                var sub = _handlers[eventName];
                foreach (var item in sub)
                {
                    var handler = Activator.CreateInstance(item);

                    if (handler == null) continue;
                    var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);

                    var @event = JsonConvert.DeserializeObject(message, eventType);

                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                }
            }
        }
    }
}
