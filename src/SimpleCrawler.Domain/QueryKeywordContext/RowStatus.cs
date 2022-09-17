namespace SimpleCrawler.Domain.QueryKeywordContext
{
    public enum RowStatus
    {
        None = 0,
        Wait = 10,
        Processing = 11,
        Processed = 12,
        Completed = 13,
        Failed = 50,
        Deleted = 51
    }
}