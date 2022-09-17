namespace SimpleCrawler.Domain.QueryKeywordContext.QueryResultSummaryAggregation
{
    public static class QueryResultSummaryFactory
    {
        public static QueryResultSummary GetQueryResultSummaryFromDto(QueryResultSummaryDto dto)
        {
            return new QueryResultSummary(dto.Id, dto.UserId, dto.Keyword, dto.TotalQuery, dto.TotalImpact, 
                dto.LastQueryDate, dto.InsertDate);
        }

        public static QueryResultSummary GetQueryResultSummaryFromDbObject(QueryResultSummaryDbObject dbObject)
        {
            return new QueryResultSummary(dbObject.Id, dbObject.UserId, dbObject.Keyword, dbObject.TotalQuery,
                dbObject.TotalImpact, dbObject.LastQueryDate, dbObject.InsertDate);
        }
    }
}