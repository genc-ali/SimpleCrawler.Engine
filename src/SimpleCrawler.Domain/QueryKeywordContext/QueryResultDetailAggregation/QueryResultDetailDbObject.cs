using System;
using System.Collections.Generic;
using SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation;

namespace SimpleCrawler.Domain.QueryKeywordContext.QueryResultDetailAggregation
{
    public class QueryResultDetailDbObject
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Keyword{ get; set; }
        public string SearchEngine { get; set; }
        public QueryPeriod QueryPeriod { get; set; }
        public List<Uri> Urls{ get; set; }
        public RowStatus RowStatus{ get; set; }
        public DateTime InsertDate { get; set; }
        
        public QueryResultDetailDbObject(Guid id, Guid userId, string keyword, string searchEngine,
            QueryPeriod queryPeriod, List<Uri> urls, RowStatus rowStatus, DateTime? insertDate)
        {
            Id = id;
            UserId = userId;
            Keyword = keyword;
            QueryPeriod = queryPeriod;
            SearchEngine = searchEngine;
            Urls = urls;
            RowStatus = rowStatus;
            InsertDate = insertDate ?? DateTime.UtcNow;
        }
    }
}