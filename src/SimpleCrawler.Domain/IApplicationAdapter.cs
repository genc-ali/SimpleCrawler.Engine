using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleCrawler.Core;
using SimpleCrawler.Core.Crawler;
using SimpleCrawler.Domain.QueryKeywordContext;
using SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation;
using SimpleCrawler.Domain.QueryKeywordContext.QueryResultDetailAggregation;

namespace SimpleCrawler.Domain
{
    public interface IApplicationAdapter
    {
        public Task<List<Uri>> QueryProcessStart(WebCrawler crawler, QueryKeywordDto queryKeywordDto);
        Task<QueryKeywordDto> SaveSearchSummary(QueryKeywordDto queryKeywordDto, QueryResultDetail queryResultDetail);
        Task<QueryKeywordDto> InsertNewQueryKeyword(QueryKeywordDto queryKeywordDto);
    }
}