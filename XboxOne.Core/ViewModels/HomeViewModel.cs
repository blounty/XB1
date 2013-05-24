using System.Collections.ObjectModel;
using Cirrious.MvvmCross.ViewModels;
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


    }
}
