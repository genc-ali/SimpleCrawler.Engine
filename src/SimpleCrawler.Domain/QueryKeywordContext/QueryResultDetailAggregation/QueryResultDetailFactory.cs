namespace SimpleCrawler.Domain.QueryKeywordContext.QueryResultDetailAggregation
{
    public static class QueryResultDetailFactory
    {
        public static QueryResultDetail GetQueryResultDetailFromDto(QueryResultDetailDto dto)
        {
            return new QueryResultDetail(dto.Id, dto.UserId, dto.Keyword, dto.SearchEngine, dto.QueryPeriod, dto.Urls,
                dto.RowStatus, dto.InsertDate);
        }

        public static QueryResultDetail GetQueryResultDetailFromDbObject(QueryResultDetailDbObject dbObject)
        {
            return new QueryResultDetail(dbObject.Id, dbObject.UserId, dbObject.Keyword, dbObject.SearchEngine,
                dbObject.QueryPeriod, dbObject.Urls, dbObject.RowStatus, dbObject.InsertDate);
        }
    }
}