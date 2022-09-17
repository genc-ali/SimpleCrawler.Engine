using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SimpleCrawler.Core.Database;
using SimpleCrawler.Core.Security;
using SimpleCrawler.Domain;
using SimpleCrawler.Domain.QueryKeywordContext;
using SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation;
using SimpleCrawler.Domain.QueryKeywordContext.QueryResultDetailAggregation;

namespace SimpleCrawler.MongoDb.Repository
{
    public class QueryKeywordRepository: IQueryKeywordRepository
    {
        private readonly SimpleCrawlerDbContext _simpleCrawlerDbContext;

        public QueryKeywordRepository(IDbContext simpleCrawlerDbContext)
        {
            _simpleCrawlerDbContext = (SimpleCrawlerDbContext) simpleCrawlerDbContext;
        }

        public async Task<QueryKeywordDbObject> SaveSearchSummaryAsync(QueryKeywordDbObject queryKeyword,
            QueryResultDetailDbObject queryResultDetail)
        {
            await _simpleCrawlerDbContext.QueryResultDetail.InsertOneAsync(queryResultDetail);

            await _simpleCrawlerDbContext.QueryKeywords.ReplaceOneAsync(
                doc=> doc.Id == queryKeyword.Id,
                options: new ReplaceOptions { IsUpsert = true },
                replacement: queryKeyword);

            await _simpleCrawlerDbContext.QueryResultSummary.ReplaceOneAsync(
                doc => doc.Id == queryKeyword.Id,
                options: new ReplaceOptions {IsUpsert = true},
                replacement: queryKeyword.QueryResultSummary);
            
            return queryKeyword;
        }

        public async Task<List<QueryKeywordDbObject>> GetAllKeywords()
        {
            var queryKeywordsQuery =
                await _simpleCrawlerDbContext.QueryKeywords.FindAsync(FilterDefinition<QueryKeywordDbObject>.Empty);

            var queryKeywordsList = queryKeywordsQuery.ToList();

            return queryKeywordsList;
        }
        
        public async Task<QueryKeywordDbObject> GetKeywordByUser(Guid userId, string queryKeyword)
        {
            var rowId = (userId.ToString() + "|" + queryKeyword).GetMd5Hash();
            
            var queryKeywordsQuery =
                await _simpleCrawlerDbContext.QueryKeywords.FindAsync(x =>
                    x.Id ==rowId &&
                    x.UserId == userId && x.Keyword == queryKeyword);

            var queryKeywordItem = queryKeywordsQuery.SingleOrDefault();

            return queryKeywordItem;
        }

        public async Task<QueryKeywordDbObject> InsertNewQueryKeyword(QueryKeywordDbObject queryKeyword)
        {
            if (await GetKeywordByUser(queryKeyword.UserId, queryKeyword.Keyword) != null)
            {
                throw new Exception("This keyword already inserted by same user.");
            }
            
            await _simpleCrawlerDbContext.QueryKeywords.InsertOneAsync(queryKeyword);
            return queryKeyword;
        }

        public async Task<bool>  DbConnectivityChek()
        {
            return await _simpleCrawlerDbContext.CanConnectAsync();
        }
        
    }
    
}