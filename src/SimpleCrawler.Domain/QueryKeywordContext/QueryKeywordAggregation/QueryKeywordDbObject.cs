using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using SimpleCrawler.Domain.QueryKeywordContext.QueryResultDetailAggregation;
using SimpleCrawler.Domain.QueryKeywordContext.QueryResultSummaryAggregation;

namespace SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation
{
    public class QueryKeywordDbObject
    {
        [BsonId]
        [JsonProperty] public string Id { get; set; }
        [JsonProperty] public Guid UserId { get; set;}
        [JsonProperty] public string Keyword { get; set;}

        [JsonProperty]
        public string TypeOfSearchEngine { get; set;}
        
        [JsonProperty] public QueryPeriod QueryPeriod { get; set;}
        
        [JsonProperty] public QueryResultSummaryDbObject QueryResultSummary { get; set;}

        [JsonProperty] public List<QueryResultDetailDbObject> QueryResultDetails { get; set;}

        [JsonProperty] public DateTime FirstQueryDate { get; set;}
        
        [JsonProperty] public DateTime NextQueryDate { get; set;}
        
        [JsonProperty] public RowStatus RowStatus { get; set;}
        [JsonProperty] public DateTime InsertDate { get; set;}
        
        
        public QueryKeywordDbObject(string id, Guid userId, string keyword, string typeOfSearchEngine,
            QueryPeriod queryPeriod, QueryResultSummaryDbObject queryResultSummary, 
            List<QueryResultDetailDbObject> queryResultDetails,  RowStatus rowStatus, 
            DateTime? firstQueryDate, DateTime? nextQueryDate, DateTime? insertDate)
        {
            Id = id;
            UserId = userId;
            Keyword = keyword;
            QueryPeriod = queryPeriod;
            TypeOfSearchEngine = typeOfSearchEngine;
            QueryResultSummary = queryResultSummary;
            QueryResultDetails = queryResultDetails;
            RowStatus = rowStatus;
            FirstQueryDate = firstQueryDate ?? DateTime.UtcNow;
            NextQueryDate = nextQueryDate ?? FirstQueryDate;
            InsertDate = insertDate ?? DateTime.UtcNow;
        }
    }
}