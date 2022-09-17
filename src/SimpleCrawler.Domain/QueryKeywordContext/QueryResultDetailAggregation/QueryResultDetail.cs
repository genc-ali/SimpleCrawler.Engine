using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SimpleCrawler.Core.Domain;
using SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation;

namespace SimpleCrawler.Domain.QueryKeywordContext.QueryResultDetailAggregation
{
    public class QueryResultDetail:EntityItem<QueryResultDetailDto, QueryResultDetailDbObject>
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

        public QueryResultDetail(Guid? id, Guid userId, string keyword, string searchEngine,
            QueryPeriod queryPeriod, List<Uri> urls, RowStatus rowStatus, DateTime? insertDate)
        {
            Id = id ?? Guid.NewGuid();
            UserId = userId;
            Keyword = keyword;
            QueryPeriod = queryPeriod;
            SearchEngine = searchEngine;
            Urls = urls;
            RowStatus = rowStatus;
            InsertDate = insertDate ?? DateTime.UtcNow;
        }
        
        public override QueryResultDetailDbObject GetDbObject()
        {
            return new QueryResultDetailDbObject(Id, UserId, Keyword, SearchEngine, QueryPeriod, Urls, RowStatus, InsertDate);
        }

        public override QueryResultDetailDto GetDtoObject()
        {
            return new QueryResultDetailDto(Id, UserId, Keyword, SearchEngine, QueryPeriod, Urls, RowStatus, InsertDate);
        }
    }
}