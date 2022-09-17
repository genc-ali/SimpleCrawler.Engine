using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SimpleCrawler.Domain.QueryKeywordContext.QueryResultDetailAggregation
{
    public class QueryResultDetailDto
    {
        [JsonProperty] public Guid Id { get; }
        [JsonProperty] public Guid UserId { get; }
        [JsonProperty] public string Keyword { get; }

        [JsonProperty]
        public string SearchEngine { get; }
        
        [JsonProperty] public QueryPeriod QueryPeriod { get; }
        
        [JsonProperty] public List<Uri> Urls { get; }
        
        [JsonProperty] public RowStatus RowStatus { get; }

        [JsonProperty] public DateTime InsertDate { get; }
        
        public QueryResultDetailDto(Guid id, Guid userId, string keyword, string searchEngine,
            QueryPeriod queryPeriod, List<Uri> urlList,  RowStatus rowStatus, DateTime? insertDate)
        {
            Id = id;
            UserId = userId;
            Keyword = keyword;
            QueryPeriod = queryPeriod;
            SearchEngine = searchEngine;
            Urls = urlList;
            RowStatus = rowStatus;
            InsertDate = insertDate ?? DateTime.UtcNow;
        }
    }
}