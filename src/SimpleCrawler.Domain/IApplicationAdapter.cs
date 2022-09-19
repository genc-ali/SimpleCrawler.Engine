using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation;
using SimpleCrawler.Domain.QueryKeywordContext.QueryResultDetailAggregation;
using SimpleCrawler.NetCore.Crawler;

namespace SimpleCrawler.Domain
{
    public interface IApplicationAdapter
    {
        public Task<List<Uri>> QueryProcessStart(WebCrawler crawler, QueryKeywordDto queryKeywordDto);
        Task<QueryKeywordDto> SaveSearchSummary(QueryKeywordDto queryKeywordDto, QueryResultDetail queryResultDetail);
        Task<QueryKeywordDto> InsertNewQueryKeyword(QueryKeywordDto queryKeywordDto);
    }
}