using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using SimpleCrawler.Core.Database;
using SimpleCrawler.Domain;
using SimpleCrawler.Domain.QueryKeywordContext;
using SimpleCrawler.Domain.QueryKeywordContext.QueryKeywordAggregation;
using SimpleCrawler.Domain.QueryKeywordContext.QueryResultDetailAggregation;
using SimpleCrawler.Domain.QueryKeywordContext.QueryResultSummaryAggregation;

namespace SimpleCrawler.MongoDb
{
    public class SimpleCrawlerDbContext : IDbContext
    {
        private IMongoDatabase Database { get; }

        public SimpleCrawlerDbContext(AppConfiguration appConfiguration)
        {
            var client = new MongoClient(appConfiguration.MongoDbConnection);
            Database = client.GetDatabase(appConfiguration.MongoDbName);
        }

        static SimpleCrawlerDbContext()
        {
            var pack = new ConventionPack {new CamelCaseElementNameConvention()};
            ConventionRegistry.Register("camel case", pack, t => true);

            BsonClassMap.RegisterClassMap<QueryKeywordDbObject>(map =>
            {
                map.AutoMap();
                map.SetIdMember(map.GetMemberMap(x => x.Id));
                map.UnmapProperty(i => i.QueryResultDetails);
                map.UnmapProperty(i => i.QueryResultSummary);
                map.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<QueryResultSummaryDbObject>(map =>
            {
                map.AutoMap();
                map.SetIdMember(map.GetMemberMap(x => x.Id));
                //map.UnmapProperty(i => i);
                map.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<QueryResultDetail>(map =>
            {
                map.AutoMap();
                map.SetIdMember(map.GetMemberMap(x => x.Id));
                //map.UnmapProperty(i => i);
                map.SetIgnoreExtraElements(true);
            });

        }

        public IMongoCollection<QueryKeywordDbObject> QueryKeywords =>
            Database.GetCollection<QueryKeywordDbObject>("QueryKeywords");

        public IMongoCollection<QueryResultSummaryDbObject> QueryResultSummary =>
            Database.GetCollection<QueryResultSummaryDbObject>("QueryResultSummary");

        public IMongoCollection<QueryResultDetailDbObject> QueryResultDetail =>
            Database.GetCollection<QueryResultDetailDbObject>("QueryResultDetail");


        public async Task<bool> CanConnectAsync()
        {
            var document = await Database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
            var result = document["ok"].ToJson();
            return result == "1" || result == "1.0";
        }
        
    }

}