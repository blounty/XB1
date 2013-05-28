using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;
using Parse;
using XboxOne.Core.Models;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace XboxOne.Core.ViewModels
{
    public class HomeViewModel 
		: MvxViewModel
    {
        const string TWEET_URI = "http://search.twitter.com/search.json";

        private string tweetQueryString = "?q=xboxone&page=1";

        private ObservableCollection<NewsItem> newsItems;
        public ObservableCollection<NewsItem> NewsItems
        {
            get { return newsItems; }
            set { newsItems = value; RaisePropertyChanged(() => this.NewsItems); }
        }

        private ObservableCollection<TwitterItem> twitterItems;

        public ObservableCollection<TwitterItem> TwitterItems
        {
            get { return twitterItems; }
            set { twitterItems = value; RaisePropertyChanged(() => this.TwitterItems); }
        }


        private bool isLoading;

        public bool IsLoading
        {
            get { return isLoading; }
            set 
            { 
                isLoading = value;
                this.RaisePropertyChanged(() => this.IsLoading);
            }
        }


        public HomeViewModel()
        {
            this.NewsItems = new ObservableCollection<NewsItem>();
            this.TwitterItems = new ObservableCollection<TwitterItem>();
        }

        public void NewsItemSelected(NewsItem newsItem)
        {
            this.ShowViewModel<WebContentViewModel>(new { contentUrl = newsItem.Url });
        }

        public void TwitterItemSelected(TwitterItem twitterItem)
        {
            this.ShowViewModel<WebContentViewModel>(new { contentUrl = string.Format("https://twitter.com/{0}/status/{1}", twitterItem.Author, twitterItem.Id) });
        }

        public void ShowPrivacyPolicy(string url)
        {
             this.ShowViewModel<WebContentViewModel>(new { contentUrl = url });
        }

        public override async void Start()
        {
            base.Start();

            await this.RefreshAllData();
           
        }

        public async Task RefreshAllData()
        {
            this.IsLoading = true;

            this.NewsItems.Clear();
            this.TwitterItems.Clear();

            this.tweetQueryString = "?q=xboxone&page=1";

            await this.LoadNews(20, 0);

            await this.LoadTweets();

        }

        public async Task LoadNews(int limit, int skip)
        {
            this.IsLoading = true;

            var query = ParseObject.GetQuery("NewsItem");

            query.OrderByDescending(x => x.CreatedAt);

            query.Limit(limit);

            query.Skip(skip);

            IEnumerable<ParseObject> result = new List<ParseObject>();

            try
            {
                result = await query.FindAsync();
            }
            catch (Exception)
            {
                var errorItem = new ParseObject("NewsItem");
                errorItem["Title"] = "error, please retry...";
                ((List<ParseObject>)result).Add(errorItem);       
            }

            result = result.OrderByDescending(x => x.CreatedAt);
        
            foreach (var newsItemParseObject in result)
            {
                var newsItem = await NewsItem.CreateFromParseObject(newsItemParseObject);
                if(!this.NewsItems.Any(x => x.Id == newsItem.Id)){
                    this.NewsItems.Add(newsItem);
                }
            }

            this.IsLoading = false;
        }


        public async Task LoadTweets()
        {
            this.IsLoading = true;

            var tweetClient = new HttpClient();

            try
            {
                var response = await tweetClient.GetAsync(string.Format("{0}{1}", TWEET_URI, this.tweetQueryString));

                var resultsJson = JObject.Parse(await response.Content.ReadAsStringAsync());

                this.tweetQueryString = resultsJson["next_page"].Value<string>();

                var tweetArrayJson = (JArray)resultsJson["results"];

                foreach (JObject tweetJson in tweetArrayJson)
                {
                    var twitterItem = new TwitterItem
                    {
                        Author = tweetJson["from_user"].Value<string>(),
                        AvatarUrl = tweetJson["profile_image_url"].Value<string>(),
                        PublishDate = tweetJson["created_at"].Value<DateTime>(),
                        Tweet = tweetJson["text"].Value<string>(),
                        Id = tweetJson["id"].Value<string>()
                    };
                    this.TwitterItems.Add(twitterItem);
                }
            }
            catch (Exception)
            {

            }

            this.IsLoading = false;
        }
    }
}
