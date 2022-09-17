using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SimpleCrawler.Core.Domain;
using SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation;
using SimpleCrawler.Domain.QueryKeywordContext.QueryResultDetailAggregation;

namespace SimpleCrawler.Domain.QueryKeywordContext.QueryResultSummaryAggregation
{
    public class QueryResultSummary:EntityItem<QueryResultSummaryDto, QueryResultSummaryDbObject>
    {
        [JsonProperty] public string Id { get; }
        [JsonProperty] public Guid UserId { get; }
        [JsonProperty] public string Keyword { get; }

        [JsonProperty] public long TotalQuery { get; set; }

        [JsonProperty] public long TotalImpact { get; set; }

        [JsonProperty] public DateTime LastQueryDate { get; set; }

        [JsonProperty] public DateTime InsertDate { get; }

        public QueryResultSummary(string id, Guid userId, string keyword, long totalQuery,
            long totalImpact, DateTime? lastQueryDate, DateTime? insertDate)
        {
            Id = id;
            UserId = userId;
            Keyword = keyword;
            TotalQuery = totalQuery;
            TotalImpact = totalImpact;
            LastQueryDate = lastQueryDate ?? DateTime.UtcNow;
            InsertDate = insertDate ?? DateTime.UtcNow;
        }
        
        public override QueryResultSummaryDbObject GetDbObject()
        {
            return new QueryResultSummaryDbObject(Id, UserId, Keyword, TotalQuery, TotalImpact, LastQueryDate, InsertDate);
        }

        public override QueryResultSummaryDto GetDtoObject()
        {
            return new QueryResultSummaryDto(Id, UserId, Keyword, TotalQuery, TotalImpact, LastQueryDate, InsertDate);
        }
    }
}