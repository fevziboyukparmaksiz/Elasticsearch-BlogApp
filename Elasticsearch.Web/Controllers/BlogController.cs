using Elasticsearch.Web.Models;
using Elasticsearch.Web.Services;
using Elasticsearch.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.Web.Controllers
{
	public class BlogController : Controller
	{
		private readonly BlogService _blogService;

		public BlogController(BlogService blogService)
		{
			_blogService = blogService;
		}

		public async Task<IActionResult> Search()
		{
			return View(await _blogService.SearchAsync(string.Empty));
		}
		[HttpPost]
		public async Task<IActionResult> Search(string searchText)
		{
			ViewBag.searchtext = searchText;

			return View(await _blogService.SearchAsync(searchText));
		}

		public IActionResult Save()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Save(BlogCreateViewModel model)
		{
			var isSuccess = await _blogService.SaveAsync(model);
			if (!isSuccess)
			{
				TempData["result"] = "kayıt başarız";
				RedirectToAction(nameof(BlogController.Save));
			}
			TempData["result"] = "kayıt başarılı";

			return RedirectToAction(nameof(BlogController.Save));
		}

	}
}
