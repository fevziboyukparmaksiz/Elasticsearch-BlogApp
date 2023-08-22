using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace Elasticsearch.Web.Extensions
{
    public static class ElasticSearchExt
    {
        public static void AddElastic(this IServiceCollection services, IConfiguration configuration)
        {
            var username = configuration.GetSection("Elastic")["username"];
            var password = configuration.GetSection("Elastic")["password"];

            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"]!)).Authentication(new BasicAuthentication(username!, password!));

            var client = new ElasticsearchClient(settings);
            services.AddSingleton(client);
        }
    }
}
