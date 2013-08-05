using System.Threading.Tasks;
using Parse;
using System;

namespace XboxOne.Core.Models
{
    public class NewsItem
        : BaseModel
    {
        public string Author { get; set; }

        public string Source { get; set; }

        public string SourceAuthor { get { return (!string.IsNullOrEmpty(this.Source) && !string.IsNullOrEmpty(this.Author)) ? string.Format("{0}/{1}", this.Source, this.Author) : (string.IsNullOrEmpty(this.Source) ? this.Author : this.Source); } }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string ImageUrl { get; set; }

        public string Url { get; set; }

        public DateTime CreatedAt { get; set; }

        public static async Task<NewsItem> CreateFromParseObject(ParseObject parseObject)
        {
            return await Task.Run<NewsItem>(() =>
                {
                    var newsItem = new NewsItem();

                    newsItem.Id = (string)parseObject.ObjectId;

                    if (parseObject.ContainsKey("Author"))
                    {
                        newsItem.Author = (string) parseObject["Author"];
                    }

                    if (parseObject.ContainsKey("ImageUrl"))
                    {
                        newsItem.ImageUrl = (string) parseObject["ImageUrl"];
                    }

                    if (parseObject.ContainsKey("Source"))
                    {
                        newsItem.Source = (string)parseObject["Source"];
                    }

                    if (parseObject.ContainsKey("Summary"))
                    {
                        newsItem.Summary = (string)parseObject["Summary"];
                    }

                    if (parseObject.ContainsKey("Title"))
                    {
                        newsItem.Title = (string)parseObject["Title"];
                    }

                    if (parseObject.ContainsKey("Url"))
                    {
                        newsItem.Url = (string)parseObject["Url"];
                    }

                    newsItem.CreatedAt = parseObject.CreatedAt.HasValue ? parseObject.CreatedAt.Value : DateTime.Now;

                    return newsItem;
                });
        }
    }
}