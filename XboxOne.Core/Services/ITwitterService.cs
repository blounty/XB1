using System;
namespace XboxOne.Core.Services
{
    public interface ITwitterService
    {
        System.Threading.Tasks.Task<bool> AuthorizeTwitter();
        System.Threading.Tasks.Task<System.Collections.Generic.List<XboxOne.Core.Models.TwitterItem>> LoadTweets(Action<System.Collections.Generic.List<XboxOne.Core.Models.TwitterItem>> callback);
        void ResetFilters();
        int ReturnCount { get; set; }
        string SearchTerm { get; set; }
    }
}
