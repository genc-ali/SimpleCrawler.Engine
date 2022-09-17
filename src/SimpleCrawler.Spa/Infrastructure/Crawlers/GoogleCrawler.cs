using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimpleCrawler.Core.Crawler;
using SimpleCrawler.Core.Security;

namespace SimpleCrawler.SinglePageApp.Infrastructure.Crawlers
{
    public class GoogleCrawler : WebCrawler
    {
        private const string SearchUrl = "https://www.google.co.uk/";
        private const string QueryString = "search?num={0}&q={1}&start={2}";
        private JsonSerializerSettings _settings;

        private readonly List<string> _filterListArray = new()
        {
            "www.google.co.uk/search?",
            "maps.google.co.uk/maps?",
            "support.google.com/",
            "accounts.google.com/",
            "www.google.co.uk/",
            "policies.google.com/",
        };

        private readonly string _filteredList;

        public GoogleCrawler()
        {
            _filteredList = '|' + string.Join('|', _filterListArray) + '|';

            /*
            var client = new WebClient();
            client.Headers.Add("Cache-Control", "no-cache");
            client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            */

            _settings = new JsonSerializerSettings
            {
                ContractResolver = new DictionaryAsArrayResolver(),
                Formatting = Formatting.Indented
            };
        }

        public override async Task<List<Uri>> GetMentions(int pageSize, string keyword,
            int pageIndex = 0, string initialHashCode = "")
        {
            try
            {
                var queryString = String.Format(QueryString, pageSize, keyword, (pageIndex - 1) * pageSize);
                var response = "";

                try
                {
                    //response = await _client.DownloadStringTaskAsync(new Uri(formattedUrl));
                    var baseAddress = new Uri(SearchUrl);
                    using var firstHandler = new HttpClientHandler() {UseCookies = false};
                    using var clientUsed = new HttpClient(firstHandler) {BaseAddress = baseAddress};
                    clientUsed.BaseAddress = baseAddress;
                    clientUsed.DefaultRequestHeaders.Accept.Clear();

                    string agent = "ClientDemo/1.0.0.1 test user agent DefaultRequestHeaders";
                    clientUsed.DefaultRequestHeaders.Add("User-Agent", agent);
                    var firstRequest = new HttpRequestMessage(HttpMethod.Get, queryString);
                    HttpResponseMessage responseUsed = await clientUsed.SendAsync(firstRequest);

                    if (responseUsed.StatusCode == HttpStatusCode.OK)
                    {
                        response = await responseUsed.Content.ReadAsStringAsync();
                    }
                }
                catch (WebException webEx)
                {
                    // service gets too many request problem. This block is unlocked by google end of the unknown period. (it may be 1 hour)
                    // system should not try next query until this period.
                    Console.WriteLine(webEx);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                Regex regex =
                    new Regex(
                        @"(?:(?:https?|ftp|file):\/\/|www\.|ftp\.)(?:\([-A-Z0-9+&@#\/%=~_|$?!:,.]*\)|[-A-Z0-9+&@#\/%=~_|$?!:,.])*(?:\([-A-Z0-9+&@#\/%=~_|$?!:,.]*\)|[A-Z0-9+&@#\/%=~_|$])",
                        RegexOptions.IgnoreCase);
                MatchCollection collection = regex.Matches(response);

                List<Uri> urlList = new List<Uri>();
                StringBuilder urlListStr = new StringBuilder("");

                foreach (Match match in collection)
                {
                    var urlAddress = match.ToString();
                    urlAddress = urlAddress.EndsWith("&amp")
                        ? urlAddress.Substring(0, urlAddress.Length - "&amp".Length)
                        : urlAddress;

                    if (!(urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                        urlAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
                    {
                        urlAddress = "https://" + urlAddress;
                    }

                    if (_filteredList.Contains('|' + urlAddress + '|'))
                        continue;

                    try
                    {
                        urlList.Add(new Uri(urlAddress));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                    urlListStr.Append(urlAddress + "\n");
                }

                var domainList = urlList.Select(q => q.Host).ToList().OrderBy(x => x).ToList();
                var currentHashCode = JsonConvert.SerializeObject(domainList, _settings).GetMd5Hash();

                if ((urlList.Count > 0) && (currentHashCode != initialHashCode))
                {
                    IEnumerable<Uri> nextPageList = await GetMentions(pageSize, keyword, ++pageIndex, currentHashCode);
                    urlList.AddRange(nextPageList);
                }

                return urlList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    class DictionaryAsArrayResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            if (objectType.GetInterfaces().Any(i => i == typeof(IDictionary) ||
                                                    (i.IsGenericType &&
                                                     i.GetGenericTypeDefinition() == typeof(IDictionary<,>))))
            {
                return base.CreateArrayContract(objectType);
            }

            return base.CreateContract(objectType);
        }
    }
}