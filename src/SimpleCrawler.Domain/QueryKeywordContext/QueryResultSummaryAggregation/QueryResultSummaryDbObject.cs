using System;

namespace SimpleCrawler.Domain.QueryKeywordContext.QueryResultSummaryAggregation
{
    public class QueryResultSummaryDbObject
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public string Keyword{ get; set; }
        
        public long TotalQuery { get; set; }
        
        public long TotalImpact{ get; set; }
        
        public DateTime LastQueryDate { get; set; }
        public DateTime InsertDate { get; set; }
        
        public QueryResultSummaryDbObject(string id, Guid userId, string keyword, long totalQuery, long totalImpact, DateTime? lastQueryDate ,DateTime insertDate)
        {
            Id = id;
            UserId = userId;
            Keyword = keyword;
            TotalQuery = totalQuery;
            TotalImpact = totalImpact;
            LastQueryDate = lastQueryDate ?? DateTime.UtcNow;
            InsertDate = insertDate;
        }
    }
}