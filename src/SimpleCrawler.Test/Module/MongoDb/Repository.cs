using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleCrawler.Domain;
using SimpleCrawler.MongoDb;
using SimpleCrawler.MongoDb.Repository;
using SimpleCrawler.NetCore.Database;
using Xunit;

namespace SimpleCrawler.Test.Module.MongoDb
{
    public class Repository
    {
        //private readonly ApplicationAdapter _applicationAdapter;
        private readonly QueryKeywordRepository _repository;

        public Repository()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            
            AppConfiguration serviceConfigurationSettings = new AppConfiguration(configuration);

            var services = new ServiceCollection();
            services.AddSingleton<AppConfiguration>();
            services.AddScoped<IDbContext, SimpleCrawlerDbContext>();

            services.AddSingleton<IQueryKeywordRepository, QueryKeywordRepository>();
            services.AddScoped<ApplicationService>();
            services.AddScoped<IApplicationAdapter, ApplicationAdapter>();

            var serviceProvider = services.BuildServiceProvider();

            _repository = new QueryKeywordRepository(new SimpleCrawlerDbContext(serviceConfigurationSettings));

            //_applicationAdapter = (ApplicationAdapter) serviceProvider.GetService<IApplicationAdapter>();
        }

        [Fact]
        public async Task DbConnectivityChek()
        {
            var result = await _repository.DbConnectivityCheck();
            Assert.True(result);
        }


    }
}