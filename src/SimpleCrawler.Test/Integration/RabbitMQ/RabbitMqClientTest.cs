using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SimpleCrawler.Core.DateTime;
using SimpleCrawler.Core.MessageQueue.RabbitMq;
using SimpleCrawler.Domain;
using SimpleCrawler.Domain.QueryKeywordContext;
using SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation;
using SimpleCrawler.SinglePageApp;
using SimpleCrawler.SinglePageApp.Infrastructure;
using SimpleCrawler.SinglePageApp.Infrastructure.Crawlers;
using SimpleCrawler.SinglePageApp.Infrastructure.MessageQueue;
using Xunit;

namespace SimpleCrawler.Test.Integration.RabbitMQ
{
    public class RabbitMqClientTest
    {

        private readonly ILogger _logger;
        private readonly AppConfiguration _appConfiguration;
        private static readonly Guid UserId = new Guid("123e4567-e89b-12d3-a456-426614174000");

        private RabbitMqClient _rabbitMqClient;

        public RabbitMqClientTest()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _appConfiguration = new AppConfiguration(configuration);
            
            var services = new ServiceCollection();
            services.AddSingleton(_ => (IConfigurationRoot) configuration);
            services.AddSingleton<AppConfiguration>();
            
            services.AddSingleton<IRabbitMqClient, RabbitMqClient>();
            var serviceProvider = services.AddLogging().BuildServiceProvider();
            
            var iLoggerFactory = serviceProvider.GetService<ILoggerFactory>();
            _logger = iLoggerFactory?.CreateLogger<RabbitMqClient>();
            
            _rabbitMqClient =  (RabbitMqClient)serviceProvider.GetRequiredService<IRabbitMqClient>();

        }

        [Theory, ClassData(typeof(RabbitMqMessages))]
        public void PushMessage(string routingKey, object message)
        {
            
            _logger.LogInformation("PushMessage, routingKey:{RoutingKey}", routingKey);
            
            string msgJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(msgJson);
            
            _rabbitMqClient.SendMessage(body, routingKey, DateTime.UtcNow.AddMinutes(10));
        }

        private class RabbitMqMessages: IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[]
                {
                    "NEW",
                    new QueryKeywordDto(UserId, "info-track", 
                        typeof(GoogleCrawler), QueryPeriod.Daily, DateTime.UtcNow.AddMinutes(5))
                },
                /*
                new object[]
                {
                    "NEW",
                    new QueryKeywordDto(UserId, "ABD", 
                        typeof(GoogleCrawler), QueryPeriod.Daily)
                },
                */
            };
 
            public IEnumerator<object[]> GetEnumerator()
            { return _data.GetEnumerator(); }
 
            IEnumerator IEnumerable.GetEnumerator()
            { return GetEnumerator(); }
        }
    }

    
}