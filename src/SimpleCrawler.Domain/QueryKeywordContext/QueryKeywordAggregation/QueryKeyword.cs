using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SimpleCrawler.Core.Domain;
using SimpleCrawler.Domain.QueryKeywordContext.QueryResultDetailAggregation;
using SimpleCrawler.Domain.QueryKeywordContext.QueryResultSummaryAggregation;

namespace SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation
{
    public class QueryKeyword: EntityItem<QueryKeywordDto, QueryKeywordDbObject>
    {
        [JsonProperty] public string Id { get; }
        [JsonProperty] public Guid UserId { get; }
        [JsonProperty] public string Keyword { get; }

        [JsonProperty]
        public string TypeOfSearchEngine { get; }
        
        [JsonProperty] public QueryPeriod QueryPeriod { get; }

        [JsonProperty] public RowStatus RowStatus { get; internal set; }

        [JsonProperty]
        public QueryResultSummary QueryResultSummary { get; internal set; }
        
        [JsonProperty]
        public List<QueryResultDetail> QueryResultDetail { get; internal set; }

        [JsonProperty] public DateTime FirstQueryDate { get; }
        
        [JsonProperty] public DateTime NextQueryDate { get; internal set; }

        [JsonProperty] public DateTime InsertDate { get; }
        
        public QueryKeyword(string id, Guid userId, string keyword, string searchEngine,
            QueryPeriod queryPeriod, RowStatus rowStatus, DateTime? firstQueryDate, DateTime nextQueryDate, DateTime? insertDate)
        {
            Id = id;
            UserId = userId;
            Keyword = keyword;
            QueryPeriod = queryPeriod;
            TypeOfSearchEngine = searchEngine;
            RowStatus = rowStatus;
            FirstQueryDate = firstQueryDate ?? DateTime.UtcNow;
            NextQueryDate = nextQueryDate;
            InsertDate = insertDate ?? DateTime.UtcNow;
        }

        public override QueryKeywordDbObject GetDbObject()
        {
            return new QueryKeywordDbObject(Id, UserId, Keyword, TypeOfSearchEngine, QueryPeriod, 
                QueryResultSummary?.GetDbObject(), QueryResultDetail?.Select(x=> x.GetDbObject()).ToList(),
                RowStatus, FirstQueryDate, NextQueryDate, InsertDate);
        }

        public override QueryKeywordDto GetDtoObject()
        {
            return new QueryKeywordDto(Id, UserId, Keyword, TypeOfSearchEngine, QueryPeriod,   
                QueryResultSummary?.GetDtoObject(), QueryResultDetail?.Select(x=> x.GetDtoObject()).ToList(),
                RowStatus, FirstQueryDate,  NextQueryDate, InsertDate);
        }

        public void UpdateSearchSummary(QueryResultDetail queryResultDetail)
        {
            QueryResultSummary ??= new QueryResultSummary(Id, UserId, Keyword, 0, 0, null, InsertDate);
            QueryResultDetail ??= new List<QueryResultDetail>();
            
            QueryResultDetail.Add(queryResultDetail);
            RowStatus = RowStatus.Completed;

            switch (QueryPeriod)
            {
                case QueryPeriod.Daily:
                case QueryPeriod.Weekly:
                    NextQueryDate = NextQueryDate.AddHours((int) QueryPeriod);
                    break;
                case QueryPeriod.Monthly:
                    NextQueryDate = NextQueryDate.AddMonths(1);
                    break;
                default:
                    NextQueryDate = NextQueryDate.AddHours(1);
                    break;
            }

            QueryResultSummary.TotalImpact += queryResultDetail.Urls.Count;
            QueryResultSummary.TotalQuery++;
            QueryResultSummary.LastQueryDate = DateTime.UtcNow;

        }
    }
}