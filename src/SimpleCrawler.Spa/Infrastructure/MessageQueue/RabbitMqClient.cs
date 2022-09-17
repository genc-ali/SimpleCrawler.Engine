using System;
using RabbitMQ.Client;
using SimpleCrawler.Core.DateTime;
using SimpleCrawler.Core.MessageQueue.RabbitMq;
using SimpleCrawler.Domain;

namespace SimpleCrawler.SinglePageApp.Infrastructure.MessageQueue
{
    public class RabbitMqClient:IRabbitMqClient
    {
        private readonly IModel _channel;
        private readonly AppConfiguration _appConfiguration;
        
        public RabbitMqClient(AppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
            var uri = new Uri(
                $"amqp://{_appConfiguration.UserName}:{_appConfiguration.Password}@{_appConfiguration.Host}:{_appConfiguration.Port}/CUSTOM_HOST");
            
            var factory = new ConnectionFactory
            {
                Uri = uri
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.ExchangeDeclare(_appConfiguration.MessageExchange, ExchangeType.Direct, true, false, null);           
            _channel.QueueDeclare(_appConfiguration.MessageQueue, true, false, false, null);           
            _channel.QueueBind(_appConfiguration.MessageQueue, _appConfiguration.MessageExchange, "NEW");
            _channel.ConfirmSelect();
        }

        public void SendMessage(byte[] body, string routingKey, DateTime addMinutes)
        {
            routingKey ??= _appConfiguration.MessageRouteKey;
            
            IBasicProperties properties = _channel.CreateBasicProperties();
            properties.Timestamp = addMinutes.ToAmqpTimestamp();
            
            PublicationAddress address = new PublicationAddress(ExchangeType.Direct, _appConfiguration.MessageExchange, routingKey);
            _channel.BasicPublish(address, properties, body); 
        }
        
    }
}