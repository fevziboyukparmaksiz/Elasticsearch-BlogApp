﻿using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
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
			newBlog.Created = DateTime.Now;
			var response = await _elasticsearchClient.IndexAsync(newBlog, x => x.Index(indexName));
			if (response.IsValidResponse) return null;

			newBlog.Id = response.Id;

			return newBlog;

		}

		public async Task<List<Blog>> SearchAsync(string searchText)
		{
			List<Action<QueryDescriptor<Blog>>> ListQuery = new();

			Action<QueryDescriptor<Blog>> matchAll = q => q.MatchAll();
			Action<QueryDescriptor<Blog>> matchContent = q => q.Match(m => m
						   .Field(f => f.Content)
						   .Query(searchText));
			Action<QueryDescriptor<Blog>> titleMatchBoolPrefix = q => q.Match(m => m
						   .Field(f => f.Title)
						   .Query(searchText));

			if (string.IsNullOrEmpty(searchText))
			{
				ListQuery.Add(matchAll);
			}
			else
			{
				ListQuery.Add(matchContent);
				ListQuery.Add(titleMatchBoolPrefix);
			}

			var result = await _elasticsearchClient.SearchAsync<Blog>(s => s.Index(indexName).Size(1000)
			   .Query(q => q
				   .Bool(b => b
					   .Should(ListQuery.ToArray()))));

			foreach (var hits in result.Hits) hits.Source.Id = hits.Id;

			return result.Documents.ToList();

		}
	}
}
