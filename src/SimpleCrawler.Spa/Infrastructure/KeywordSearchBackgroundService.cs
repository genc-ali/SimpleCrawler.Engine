using System;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleCrawler.Core;
using SimpleCrawler.Core.Crawler;
using SimpleCrawler.Domain;
using SimpleCrawler.Domain.QueryKeywordContext;
using SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation;
using SimpleCrawler.Domain.QueryKeywordContext.QueryResultDetailAggregation;

namespace SimpleCrawler.SinglePageApp.Infrastructure
{
    public class KeywordSearchBackgroundService : RabbitListener
    {
        private readonly ILogger<KeywordSearchBackgroundService> _logger;

        // Because the Process function is a delegate callback, if you inject other services directly, they are not in one scope.
        // To invoke other Service instances, you can only use IServiceProvider CreateScope to retrieve instance objects
        private readonly IApplicationAdapter _applicationAdapter;
        private readonly IServiceScope _scope;

        public KeywordSearchBackgroundService(IServiceProvider services, 
            AppConfiguration appConfiguration,
            ILogger<KeywordSearchBackgroundService> logger) : base(appConfiguration)
        {
            _logger = logger;
            _scope = services.CreateScope();
            _applicationAdapter = _scope.ServiceProvider.GetRequiredService<IApplicationAdapter>();

        }

        protected override async Task<QueryKeywordDto> Process(string message)
        {
            var queryKeywordDto = JsonConvert.DeserializeObject<QueryKeywordDto>(message);
            
            try
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();
                Type searchEngineType = entryAssembly?.GetType(queryKeywordDto?.TypeOfSearchEngine ?? throw new InvalidOperationException());
                
                var webCrawler = (WebCrawler) _scope.ServiceProvider.GetRequiredService(searchEngineType ?? throw new InvalidOperationException());
                var urlList = await _applicationAdapter.QueryProcessStart(webCrawler, queryKeywordDto);

                QueryResultDetail queryResultDetail = new QueryResultDetail(null, queryKeywordDto.UserId,
                    queryKeywordDto.Keyword, queryKeywordDto.TypeOfSearchEngine, queryKeywordDto.QueryPeriod, urlList,
                    RowStatus.Completed, null);
                
                var saveResult = await _applicationAdapter.SaveSearchSummary(queryKeywordDto, queryResultDetail);
                
                return saveResult;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Process fail,error:{ExMessage},stackTrace:{ExStackTrace},message:{QueMessage}",
                    ex.Message, ex.StackTrace, message);
                
                _logger.LogError(-1, ex, "Process fail");
                return null;
            }

        }
    }
}