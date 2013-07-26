using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Bookmarks;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XboxOne.Core.Models;
using XboxOne.Core.Services;

namespace XboxOne.Core.ViewModels
{
    public class GameViewModel
        : MvxViewModel
    {
        private string gameId;
        private IGameService gameService;
        private INewsService newsService;
        private IVideoService videoService;
        private Game game;

        public Game Game
        {
            get { return game; }
            set 
            { 
                game = value;
                this.RaisePropertyChanged(() => this.Game);
            }
        }

        private ObservableCollection<NewsItem> newsItems;
        public ObservableCollection<NewsItem> NewsItems
        {
            get { return newsItems; }
            set { newsItems = value; RaisePropertyChanged(() => this.NewsItems); }
        }

        private ObservableCollection<Video> videos;
        public ObservableCollection<Video> Videos
        {
            get { return videos; }
            set { videos = value; RaisePropertyChanged(() => this.Videos); }
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

        public GameViewModel(IGameService gameService, INewsService newsService, IVideoService videoService)
        {
            this.gameService = gameService;
            this.newsService = newsService;
            this.videoService = videoService;
            this.NewsItems = new ObservableCollection<NewsItem>();
            this.Videos = new ObservableCollection<Video>();
        }

        public async void Init(string gameId)
        {
            this.gameId = gameId;
        }

        public override void Start()
        {
            base.Start();
            this.RefreshAllData();
        }


        public async Task RefreshAllData()
        {
            this.IsLoading = true;

            this.NewsItems.Clear();

            this.Game = await this.gameService.LoadGameById(this.gameId);

            await this.LoadNews();

            await this.LoadVideos();

            this.IsLoading = false;
        }

        private async Task LoadNews()
        {
            var newsItems = await this.newsService.LoadNewsByGameId(this.gameId);

            foreach (var newsItem in newsItems)
            {
                if (!this.NewsItems.Any(x => x.Id == newsItem.Id))
                {
                    this.NewsItems.Add(newsItem);
                }
            }
        }

        private async Task LoadVideos()
        {
            var videos = await this.videoService.LoadVideosByGameId(this.gameId);

            foreach (var video in videos)
            {
                if (!this.Videos.Any(x => x.Id == video.Id))
                {
                    this.Videos.Add(video);
                }
            }
        }

        public void NewsItemSelected(NewsItem newsItem)
        {
            this.ShowViewModel<WebContentViewModel>(new { contentUrl = newsItem.Url });
        }

        public void VideoSelected(Video video)
        {
            this.ShowViewModel<YouTubeContentViewModel>(new { videoId = video.VideoId });
        }

        public async Task AddSecondaryTile()
        {
            Task.Run(() =>
            {
                var librarian = Mvx.Resolve<IMvxBookmarkLibrarian>();

                if (!librarian.HasBookmark(this.gameId))
                {
                    var metaData = new MvxBookmarkMetadata 
                    {
                        BackgroundImageUri = new Uri(this.Game.ImageUrl),
                        Title = string.Format("XB1 - {0}", game.Name),
                        BackContent = this.Game.PublisherDeveloper
                    };
                    librarian.AddBookmark(typeof(GameViewModel), this.gameId, metaData, new Dictionary<string, string> { { "gameId", this.gameId } });
                }
            });
        }
    }
}
