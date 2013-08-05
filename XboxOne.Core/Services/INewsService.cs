using System;
namespace XboxOne.Core.Services
{
    public interface INewsService
    {
        System.Threading.Tasks.Task<System.Collections.Generic.List<XboxOne.Core.Models.NewsItem>> LoadNews(int limit, int skip);
        System.Threading.Tasks.Task<System.Collections.Generic.List<XboxOne.Core.Models.NewsItem>> LoadNewsByGameId(string gameId);
    }
}
