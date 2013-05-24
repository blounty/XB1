using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;
using Parse;
using XboxOne.Core.Models;

namespace XboxOne.Core.ViewModels
{
    public class HomeViewModel 
		: MvxViewModel
    {
        private ObservableCollection<NewsItem> newsItems;
        public ObservableCollection<NewsItem> NewsItems
        {
            get { return newsItems; }
            set { newsItems = value; RaisePropertyChanged(() => this.NewsItems); }
        }

        public override async void Start()
        {
            base.Start();

            IEnumerable<ParseObject> result = await ParseObject.GetQuery("NewsItem").OrderByDescending("createdAt").Limit(20).FindAsync();

            this.NewsItems = new ObservableCollection<NewsItem>();

            foreach (var newsItemParseObject in result)
            {
                this.NewsItems.Add(await NewsItem.CreateFromParseObject(newsItemParseObject));
            }
        }
    }
}
