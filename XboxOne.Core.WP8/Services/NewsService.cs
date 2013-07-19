using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XboxOne.Core.Models;

namespace XboxOne.Core.WP8.Services
{
    public class NewsService
    {
        public async Task<List<NewsItem>> LoadNews(int limit, int skip)
        {
            var query = ParseObject.GetQuery("NewsItem");

            query.OrderByDescending(x => x.CreatedAt);

            query.Limit(limit);

            query.Skip(skip);

            IEnumerable<ParseObject> result = new List<ParseObject>();

            try
            {
                result = await query.FindAsync();
            }
            catch (Exception ex)
            {
                var errorItem = new ParseObject("NewsItem");
                errorItem["Title"] = "error, please retry...";
                ((List<ParseObject>)result).Add(errorItem);
            }

            result = result.OrderByDescending(x => x.CreatedAt).Take(limit);

            var newsItems = new List<NewsItem>();
            foreach (var newsItemParseObject in result)
            {
                var newsItem = await NewsItem.CreateFromParseObject(newsItemParseObject);
                newsItems.Add(newsItem);
            }

            return newsItems;
        }
    }
}
