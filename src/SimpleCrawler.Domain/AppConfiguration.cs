using System;
using Microsoft.Extensions.Configuration;

namespace SimpleCrawler.Domain
{
    public class AppConfiguration
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string MessageQueue { get; set; }
        
        public string MessageExchange { get; set; }
        public string MessageRouteKey { get; set; }
        public string MongoDbConnection { get; set; }
        public string MongoDbName { get; set; }

        public AppConfiguration(IConfigurationRoot configurationRoot)
        {
            Host = configurationRoot["ASPNETCORE_RabbitHost"].ToString();
            UserName = configurationRoot["ASPNETCORE_RabbitUserName"];
            Password = configurationRoot["ASPNETCORE_RabbitPassword"];
            Port = Convert.ToInt32(configurationRoot["ASPNETCORE_RabbitPort"]?? "0");
            MessageQueue = configurationRoot["ASPNETCORE_RabbitMessageQueue"];
            MessageExchange = configurationRoot["ASPNETCORE_RabbitMessageExchange"];
            MessageRouteKey= configurationRoot["ASPNETCORE_RabbitMessageRouteKey"];
            MongoDbConnection= configurationRoot["ASPNETCORE_MongoDbConnection"];
            MongoDbName = configurationRoot["ASPNETCORE_MongoDbName"];
        }
        
    }
}