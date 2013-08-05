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
using Newtonsoft.Json.Linq;
using System.Text;
using XboxOne.Core.Services;

namespace XboxOne.Core.ViewModels
{
    public class HomeViewModel 
		: MvxViewModel
    {
        private ITwitterService twitterService;

        private IGameService gameService;

        private INewsService newsService;
        private ObservableCollection<NewsItem> newsItems;
        public ObservableCollection<NewsItem> NewsItems
        {
            get { return newsItems; }
            set { newsItems = value; RaisePropertyChanged(() => this.NewsItems); }
        }

        private ObservableCollection<Game> games;
        public ObservableCollection<Game> Games
        {
            get { return games; }
            set { games = value; RaisePropertyChanged(() => this.Games); }
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


        public HomeViewModel(IGameService gameService, ITwitterService twitterService, INewsService newsService)
        {
            this.newsService = newsService;
            this.twitterService = twitterService;
            this.twitterService.SearchTerm = "xboxone";
            this.twitterService.ReturnCount = 10;

            this.NewsItems = new ObservableCollection<NewsItem>();
            this.TwitterItems = new ObservableCollection<TwitterItem>();
            this.Games = new ObservableCollection<Game>();
            this.gameService  = gameService;
        }

        public void NewsItemSelected(NewsItem newsItem)
        {
            this.ShowViewModel<WebContentViewModel>(new { contentUrl = newsItem.Url });
        }

        public void GameSelected(Game game)
        {
            this.ShowViewModel<GameViewModel>(new { gameId = game.Id });
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
            this.Games.Clear();

            this.twitterService.ResetFilters();

            await this.LoadNews(20, 0);

            await this.LoadGames();

            await this.AuthorizeTwitter();

            await this.LoadTweets();

        }

        public async Task LoadNews(int limit, int skip)
        {
            this.IsLoading = true;

            var newsItems = await this.newsService.LoadNews(limit, skip);

            foreach (var newsItem in newsItems)
            {
                if (!this.NewsItems.Any(x => x.Id == newsItem.Id))
                {
                    this.NewsItems.Add(newsItem);
                }
            }
            this.IsLoading = false;
        }

        public async Task LoadGames()
        {
            this.IsLoading = true;

            var games = await gameService.LoadGames();

            foreach (var game in games)
            {
                
                if (!this.Games.Any(x => x.Id == game.Id))
                {
                    this.Games.Add(game);
                }
            }

            this.IsLoading = false;
        }

        private async Task AuthorizeTwitter()
        {
            this.IsLoading = true;

            var success = await this.twitterService.AuthorizeTwitter();

            if (!success)
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
            this.IsLoading = true;

            this.twitterService.LoadTweets((tweets) => {
                foreach (var tweet in tweets)
                {
                    this.Dispatcher.RequestMainThreadAction(() =>
                    {
                        this.TwitterItems.Add(tweet);
                    });
                }
                this.IsLoading = false;
            });
        }
    }
}
