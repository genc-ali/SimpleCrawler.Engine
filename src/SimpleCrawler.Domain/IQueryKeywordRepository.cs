using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleCrawler.Domain.QueryKeywordContext;
using SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation;
using SimpleCrawler.Domain.QueryKeywordContext.QueryResultDetailAggregation;

namespace SimpleCrawler.Domain
{
    public interface IQueryKeywordRepository
    {
        Task<QueryKeywordDbObject> SaveSearchSummaryAsync(QueryKeywordDbObject queryKeyword,
            QueryResultDetailDbObject queryResultDetail);
        Task<List<QueryKeywordDbObject>> GetAllKeywords();
        Task<QueryKeywordDbObject> InsertNewQueryKeyword(QueryKeywordDbObject queryKeyword);
        Task<QueryKeywordDbObject> GetKeywordByUser(Guid userId, string queryKeyword);
    }
}