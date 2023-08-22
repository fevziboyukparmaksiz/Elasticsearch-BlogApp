using System.Text.Json.Serialization;

namespace Elasticsearch.Web.ViewModels
{
    public class BlogCreateViewModel
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public List<string> Tags { get; set; } = new();
    }
}
