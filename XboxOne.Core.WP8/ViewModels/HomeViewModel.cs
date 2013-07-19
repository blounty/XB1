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
using System.Text;
using XboxOne.Core.WP8.Services;

namespace XboxOne.Core.ViewModels
{
    public class HomeViewModel 
		: MvxViewModel
    {
        const string TWEET_URI = "https://api.twitter.com/1.1/search/tweets.json";

        const string TWEET_AUTH_URL = "https://api.twitter.com/oauth2/token";

        private string twitterAccessToken = "";

        private string tweetQueryString = "?q=xboxone";

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

            this.tweetQueryString = "?q=xboxone&count=20";

            await this.LoadNews(20, 0);

            await this.AuthorizeTwitter();

            await this.LoadTweets();

        }

        public async Task LoadNews(int limit, int skip)
        {
            this.IsLoading = true;

            var newsService = new NewsService();
            var newsItems = await newsService.LoadNews(limit, skip);

            newsItems.ForEach((nI =>
            {
                if (!this.NewsItems.Any(x => x.Id == nI.Id))
                {
                    this.NewsItems.Add(nI);
                }
            }));
            this.IsLoading = false;
           
        }
        private async Task AuthorizeTwitter()
        {
            this.IsLoading = true;

            try
            {
                var authTokens = "QUZksaqAdqZFxefaQ5cPQ:9vNnFn1SrtUAoWa1eNXjTFNpySQN2b5icTs10uDE4g";

                var base64AuthTokens = Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(authTokens));

                var twitterAuthClient = new HttpClient();

                twitterAuthClient.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", base64AuthTokens));
                var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = await twitterAuthClient.PostAsync(TWEET_AUTH_URL, content);

                var responseJson = JValue.Parse(await response.Content.ReadAsStringAsync());

                this.twitterAccessToken = responseJson["access_token"].ToString();
            }
            catch (Exception)
            {
                this.TwitterItems.Clear();
                var twitterItem = new TwitterItem
                {
                    Author = "",
                    AvatarUrl = "",
                    Tweet = "error, please retry...",
                    Id = ""
                };
                
                this.TwitterItems.Add(twitterItem);
            }
            this.IsLoading = false;

        }

       


        public async Task LoadTweets()
        {
            if (string.IsNullOrEmpty(this.twitterAccessToken))
            {
                return;
            }
            this.IsLoading = true;

            var tweetClient = new HttpClient();

            tweetClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.twitterAccessToken);
            try
            {
                var response = await tweetClient.GetAsync(string.Format("{0}{1}", TWEET_URI, this.tweetQueryString));

                var jsonString = await response.Content.ReadAsStringAsync();
                var resultsJson = JObject.Parse(jsonString);

               

                var tweetArrayJson = (JArray)resultsJson["statuses"];

                foreach (JObject tweetJson in tweetArrayJson)
                {
                    var twitterItem = new TwitterItem
                    {
                        Author = tweetJson["user"]["screen_name"].Value<string>(),
                        AvatarUrl = tweetJson["user"]["profile_image_url"].Value<string>(),
                        Tweet = tweetJson["text"].Value<string>(),
                        Id = tweetJson["id"].Value<string>()
                    };
                    var createdDateString = tweetJson["created_at"].Value<string>();

                    twitterItem.PublishDate = createdDateString.Remove(createdDateString.Length - 11);
                    this.TwitterItems.Add(twitterItem);
                }
            }
            catch (Exception)
            {
                this.TwitterItems.Clear();
                var twitterItem = new TwitterItem
                {
                    Author = "",
                    AvatarUrl = "",
                    Tweet = "error, please retry...",
                    Id = ""
                };

                this.TwitterItems.Add(twitterItem);
            }

            this.tweetQueryString = string.Format("?q=xboxone&max_id={0}&count=20", (Convert.ToInt64(this.TwitterItems.Last().Id) - 1));

            this.IsLoading = false;
        }
    }
}
