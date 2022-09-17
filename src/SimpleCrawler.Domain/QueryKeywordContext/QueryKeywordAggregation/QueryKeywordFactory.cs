namespace SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation
{
    public static class QueryKeywordFactory
    {
        public static QueryKeyword GetQueryKeywordFromDto(QueryKeywordDto dto)
        {
            return new QueryKeyword(dto.Id, dto.UserId, dto.Keyword, 
                dto.TypeOfSearchEngine, dto.QueryPeriod, dto.RowStatus, dto.FirstQueryDate, dto.NextQueryDate, dto.InsertDate);
        }

        public static QueryKeyword GetQueryKeywordFromDbObject(QueryKeywordDbObject dbObject)
        {
            return new QueryKeyword(dbObject.Id, dbObject.UserId, dbObject.Keyword, 
                dbObject.TypeOfSearchEngine, dbObject.QueryPeriod, dbObject.RowStatus, dbObject.FirstQueryDate, dbObject.NextQueryDate, dbObject.InsertDate);
        }
    }
}