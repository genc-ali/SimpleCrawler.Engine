namespace SimpleCrawler.Domain.QueryKeywordContext
{
    public enum QueryPeriod
    {
        None = 0,
        Daily = 24,
        Weekly = 168, // 7*24,
        Monthly = 720 //30*24
    }
}