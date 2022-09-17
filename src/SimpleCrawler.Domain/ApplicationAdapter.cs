using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Wrap;
using SimpleCrawler.Core;
using SimpleCrawler.Core.Crawler;
using SimpleCrawler.Core.MessageQueue.RabbitMq;
using SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation;
using SimpleCrawler.Domain.QueryKeywordContext.QueryResultDetailAggregation;

namespace SimpleCrawler.Domain
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ApplicationAdapter:IApplicationAdapter
    {
        private static AsyncPolicyWrap ApplicationPolicy { get; set; }
        private readonly ApplicationService _applicationService;
        private readonly IRabbitMqClient _rabbitMqClient;
        public ApplicationAdapter(ApplicationService applicationService, IRabbitMqClient rabbitMqClient)
        {
            _applicationService = applicationService;
            _rabbitMqClient = rabbitMqClient;
        }

        
        static ApplicationAdapter()
        {
            var waitAndRetryPolicy = Policy.Handle<Exception>(e => e is BrokenCircuitException)
                .WaitAndRetryAsync(10, attempt =>
                {
                    if (attempt < 5)
                        return TimeSpan.FromMilliseconds(attempt * 1000);
                    return TimeSpan.FromSeconds(attempt - 3);
                });

            var circuitBreakerPolicy = Policy.Handle<Exception>(e => !(e is ApplicationException))
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));

            ApplicationPolicy = circuitBreakerPolicy.WrapAsync(waitAndRetryPolicy);
        }
        
        public async Task<List<Uri>> QueryProcessStart(WebCrawler crawler, QueryKeywordDto queryKeywordDto)
        {
            try
            {
                return await _applicationService.QueryProcessStart(crawler, queryKeywordDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<QueryKeywordDto> SaveSearchSummary(QueryKeywordDto queryKeywordDto,
            QueryResultDetail queryResultDetail)
        {
            try
            {
                QueryKeyword queryKeyword = QueryKeywordFactory.GetQueryKeywordFromDto(queryKeywordDto);
                queryKeyword.UpdateSearchSummary(queryResultDetail);
                
                var response = await _applicationService.SaveSearchSummary(queryKeyword, queryResultDetail);

                return queryKeyword.GetDtoObject();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<QueryKeywordDto> InsertNewQueryKeyword(QueryKeywordDto queryKeywordDto)
        {
            var queryKeyword = QueryKeywordFactory.GetQueryKeywordFromDto(queryKeywordDto);
            QueryKeyword response = await _applicationService.InsertNewQueryKeyword(queryKeyword.GetDbObject());

            string keywordDto = JsonConvert.SerializeObject(queryKeywordDto);
            var body = Encoding.UTF8.GetBytes(keywordDto);
            _rabbitMqClient.SendMessage(body, default, queryKeywordDto.FirstQueryDate);
            
            return response?.GetDtoObject();
        }

        public async Task<List<QueryKeywordDto>> GetAllKeywords()
        {
            List<QueryKeyword> response = await _applicationService.GetAllKeywords();
            return response.Select(x => x.GetDtoObject()).ToList();
        }


        public async Task<QueryKeywordDto> GetKeywordByUser(Guid userId, string queryKeyword)
        {
            QueryKeyword response = await _applicationService.GetKeywordByUser(userId, queryKeyword);
            return response.GetDtoObject();
        }
    }
}