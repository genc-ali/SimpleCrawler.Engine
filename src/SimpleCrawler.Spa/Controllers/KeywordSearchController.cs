using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleCrawler.Domain;
using SimpleCrawler.Domain.QueryKeywordContext;
using SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation;
using SimpleCrawler.SinglePageApp.Infrastructure.Crawlers;

namespace SimpleCrawler.SinglePageApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeywordSearchController : ControllerBase
    {
        private readonly IApplicationAdapter _applicationAdapter;
        private readonly ILogger<KeywordSearchController> _logger;

        public KeywordSearchController(IApplicationAdapter applicationAdapter, ILogger<KeywordSearchController> logger)
        {
            _applicationAdapter = applicationAdapter;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<QueryKeywordDto> Get()
        {
            return null;
        }
        
        [HttpPost]
        public async Task<QueryKeywordDto> InsertNewKeyword(Guid userId, string keyword, DateTime firstQueryTime)
        {
            QueryKeywordDto queryKeywordDto =
                new QueryKeywordDto(userId, keyword, typeof(GoogleCrawler), QueryPeriod.Daily, firstQueryTime);

            return await _applicationAdapter.InsertNewQueryKeyword(queryKeywordDto);
        }

    }
}