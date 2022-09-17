using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleCrawler.Core.Crawler;
using SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation;
using SimpleCrawler.Domain.QueryKeywordContext.QueryResultDetailAggregation;

namespace SimpleCrawler.Domain
{
    public class ApplicationService
    {
        private readonly IQueryKeywordRepository _queryKeywordRepository;

        public ApplicationService(IQueryKeywordRepository queryKeywordRepository)
        {
            _queryKeywordRepository = queryKeywordRepository;
        }

        public async Task<List<Uri>> QueryProcessStart(WebCrawler webCrawler, QueryKeywordDto queryKeywordDto)
        {
            try
            {
                return await webCrawler.GetMentions(100, queryKeywordDto.Keyword, 1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<QueryKeyword> SaveSearchSummary(QueryKeyword queryKeywordDto,
            QueryResultDetail queryResultDetail)
        {
            try
            {
                var response =  await _queryKeywordRepository.SaveSearchSummaryAsync(queryKeywordDto.GetDbObject(), queryResultDetail.GetDbObject());
                var  queryKeyword = QueryKeywordFactory.GetQueryKeywordFromDbObject(response);
                return queryKeyword;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<QueryKeyword>> GetAllKeywords()
        {
            var response = await _queryKeywordRepository.GetAllKeywords();
            List<QueryKeyword> keywordList = response.Select(QueryKeywordFactory.GetQueryKeywordFromDbObject).ToList();
            return keywordList;
        }

        public async Task<QueryKeyword> InsertNewQueryKeyword(QueryKeywordDbObject getDbObject)
        {
            var keywordItem = await _queryKeywordRepository.InsertNewQueryKeyword(getDbObject);
            return QueryKeywordFactory.GetQueryKeywordFromDbObject(keywordItem);
        }

        public async Task<QueryKeyword> GetKeywordByUser(Guid userId, string queryKeyword)
        {
            QueryKeywordDbObject keywordItem = await _queryKeywordRepository.GetKeywordByUser(userId, queryKeyword);
            return QueryKeywordFactory.GetQueryKeywordFromDbObject(keywordItem);
        }
    }
}