using System.Threading.Tasks;
using Parse;

namespace XboxOne.Core.Models
{
    public class NewsItem
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string Source { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string ImageUrl { get; set; }

        public string Url { get; set; }

        public static async Task<NewsItem> CreateFromParseObject(ParseObject parseObject)
        {
            return await Task.Run<NewsItem>(() =>
                {
                    var newsItem = new NewsItem();
                    if (!string.IsNullOrEmpty((string) parseObject["Author"]))
                    {
                        newsItem.Author = (string) parseObject["Author"];
                    }

                    if (!string.IsNullOrEmpty((string) parseObject["ImageUrl"]))
                    {
                        newsItem.ImageUrl = (string) parseObject["ImageUrl"];
                    }

                    if (!string.IsNullOrEmpty((string)parseObject["Source"]))
                    {
                        newsItem.Source = (string)parseObject["Source"];
                    }

                    if (!string.IsNullOrEmpty((string)parseObject["Summary"]))
                    {
                        newsItem.Summary = (string)parseObject["Summary"];
                    }

                    if (!string.IsNullOrEmpty((string)parseObject["Title"]))
                    {
                        newsItem.Title = (string)parseObject["Title"];
                    }

                    if (!string.IsNullOrEmpty((string)parseObject["Url"]))
                    {
                        newsItem.Url = (string)parseObject["Url"];
                    }
                    return newsItem;
                });
        }
    }
}