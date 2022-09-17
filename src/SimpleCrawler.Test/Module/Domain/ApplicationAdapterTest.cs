using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleCrawler.Core.Database;
using SimpleCrawler.Core.MessageQueue.RabbitMq;
using SimpleCrawler.Domain;
using SimpleCrawler.Domain.QueryKeywordContext;
using SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation;
using SimpleCrawler.MongoDb;
using SimpleCrawler.MongoDb.Repository;
using SimpleCrawler.SinglePageApp.Infrastructure;
using SimpleCrawler.SinglePageApp.Infrastructure.Crawlers;
using SimpleCrawler.SinglePageApp.Infrastructure.MessageQueue;
using Xunit;

namespace SimpleCrawler.Test.Module.Domain
{
    public class ApplicationAdapterTest
    {
        private static readonly Guid UserId = new Guid("123e4567-e89b-12d3-a456-426614174000");
        
        private readonly ApplicationAdapter _applicationAdapter;
        public ApplicationAdapterTest()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            
            AppConfiguration serviceConfigurationSettings = new AppConfiguration(configuration);

            var services = new ServiceCollection();
           
            services.AddSingleton(_ => (IConfigurationRoot) configuration);
            services.AddSingleton<AppConfiguration>();
            
            services.AddSingleton(_ => (IDbContext) new SimpleCrawlerDbContext(serviceConfigurationSettings));
            
            services.AddSingleton<IQueryKeywordRepository, QueryKeywordRepository>();
            services.AddScoped<ApplicationService>();
            services.AddScoped<IApplicationAdapter, ApplicationAdapter>();
            services.AddSingleton<IRabbitMqClient, RabbitMqClient>();
            services.AddSingleton<GoogleCrawler>();
            
            var serviceProvider = services.BuildServiceProvider();

            _applicationAdapter = (ApplicationAdapter) serviceProvider.GetRequiredService<IApplicationAdapter>();
        }

        [Fact]
        public async Task GetAllKeywords()
        {
            List<QueryKeywordDto> queryKeyword = await _applicationAdapter.GetAllKeywords();
            Assert.True(queryKeyword!=null);
        }
        
        [Fact]
        public async Task GetKeywordByUser()
        {
            QueryKeywordDto queryKeyword = new QueryKeywordDto(UserId, "ABD", typeof(GoogleCrawler), QueryPeriod.Daily, DateTime.UtcNow);
            QueryKeywordDto queryKeywordResponse = await _applicationAdapter.GetKeywordByUser(UserId, queryKeyword.Keyword);
            Assert.True(queryKeywordResponse!=null);
        }
        
        [Fact]
        public async Task InsertNewQueryKeyword()
        {
            QueryKeywordDto queryKeyword = new QueryKeywordDto(UserId, "ABD-2", typeof(GoogleCrawler), QueryPeriod.Daily,  DateTime.UtcNow);
            QueryKeywordDto queryKeywordResponse = await _applicationAdapter.InsertNewQueryKeyword(queryKeyword);
            Assert.True(queryKeywordResponse!=null);
        }
        
        [Fact]
        public async Task InsertSameQueryKeyword()
        {
            string keyWord = "ABD" + new string('a', 5000);
            
            QueryKeywordDto queryKeyword = new QueryKeywordDto(UserId, keyWord, typeof(GoogleCrawler), QueryPeriod.Daily,  DateTime.UtcNow);
            QueryKeywordDto queryKeywordResponse1 = await _applicationAdapter.InsertNewQueryKeyword(queryKeyword);
            Assert.True(queryKeywordResponse1!=null);
            
            await Assert.ThrowsAsync<Exception>(() =>  _applicationAdapter.InsertNewQueryKeyword(queryKeyword));
            
        }
        
        
        
        
    }
}