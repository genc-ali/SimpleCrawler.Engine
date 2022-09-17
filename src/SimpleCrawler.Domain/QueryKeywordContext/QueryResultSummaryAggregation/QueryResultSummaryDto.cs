using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SimpleCrawler.Domain.QueryKeywordContext.QueryResultSummaryAggregation
{
    public class QueryResultSummaryDto
    {
        [JsonProperty] public string Id { get; }
        [JsonProperty] public Guid UserId { get; }
        [JsonProperty] public string Keyword { get; }

        [JsonProperty] public long TotalQuery { get; }
        
        [JsonProperty] public long TotalImpact { get; }
        
        [JsonProperty] public DateTime LastQueryDate { get; }

        [JsonProperty] public DateTime InsertDate { get; }
        
        public QueryResultSummaryDto(string id, Guid userId, string keyword, long totalQuery,
            long totalImpact,  DateTime? lastQueryDate, DateTime? insertDate)
        {
            Id = id;
            UserId = userId;
            Keyword = keyword;
            TotalQuery = totalQuery;
            TotalImpact = totalImpact;
            LastQueryDate = insertDate ?? DateTime.UtcNow;
            InsertDate = insertDate ?? DateTime.UtcNow;
        }
    }
}