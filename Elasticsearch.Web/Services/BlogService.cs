﻿using Elasticsearch.Web.Models;
using Elasticsearch.Web.Repositories;
using Elasticsearch.Web.ViewModels;

namespace Elasticsearch.Web.Services
{
	public class BlogService
	{
		private readonly BlogRepository _blogRepository;

		public BlogService(BlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		public async Task<bool> SaveAsync(BlogCreateViewModel model)
		{
			var newBlog = new Blog
			{
				Title = model.Title,
				Content = model.Content,
				UserId = Guid.NewGuid(),
				Tags = model.Tags.Split(",")
			};

			var isCreatedBlog = await _blogRepository.SaveAsync(newBlog);


			return isCreatedBlog != null;
		}

		public async Task<List<BlogViewModel>> SearchAsync(string searchText)
		{
			var blogList = await _blogRepository.SearchAsync(searchText);

			return blogList.Select(b => new BlogViewModel
			{
				Id = b.Id,
				Content = b.Content,
				Title = b.Title,
				Created = b.Created.ToShortDateString(),
				Tags = String.Join(",", b.Tags),
				UserId = b.UserId.ToString()
			}).ToList();
		}
	}
}
