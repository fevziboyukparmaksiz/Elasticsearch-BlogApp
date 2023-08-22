using Elastic.Clients.Elasticsearch;
using Elasticsearch.Web.Models;

namespace Elasticsearch.Web.Repositories
{
    public class BlogRepository
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        private const string indexName = "blog";
        public BlogRepository(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        public async Task<Blog?> SaveAsync(Blog newBlog)
        {
            var response = await _elasticsearchClient.IndexAsync(newBlog, x => x.Index(indexName));
            if (response.IsValidResponse) return null;

            newBlog.Id = response.Id;

            return newBlog;

        }

        public async Task<List<Blog>> SearchAsync(string searchText)
        {
            var result = await _elasticsearchClient.SearchAsync<Blog>(s => s.Index(indexName).Size(1000)
               .Query(q => q
                   .Bool(b => b
                       .Should(
                       s => s.Match(m => m
                           .Field(f => f.Content)
                           .Query(searchText)),
                       s => s.MatchBoolPrefix(p => p
                           .Field(f => f.Title)
                           .Query(searchText))))));


            foreach (var hits in result.Hits) hits.Source.Id = hits.Id;

            return result.Documents.ToList();



        }
    }
}
