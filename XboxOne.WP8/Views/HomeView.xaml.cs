using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Data;
using XboxOne.Core.Models;
using XboxOne.Core.ViewModels;

namespace XboxOne.WP8.Views
{
    public partial class HomeView : MvxPhonePage
    {
        private int skip = 0;
        public HomeView()
        {
            InitializeComponent();
            this.Loaded += HomeView_Loaded;
            this.NewsList.ItemRealized += NewsList_ItemRealized;
            this.TweetsList.ItemRealized += TweetsList_ItemRealized;
        }

        void HomeView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var progressIndicator = SystemTray.ProgressIndicator;
            if (progressIndicator != null)
            {
                return;
            }

            progressIndicator = new ProgressIndicator();

            SystemTray.SetProgressIndicator(this, progressIndicator);

            var binding = new Binding("IsLoading") { Source = this.ViewModel };
            BindingOperations.SetBinding(
                progressIndicator, ProgressIndicator.IsVisibleProperty, binding);

            binding = new Binding("IsLoading") { Source = this.ViewModel };
            BindingOperations.SetBinding(
                progressIndicator, ProgressIndicator.IsIndeterminateProperty, binding);

            progressIndicator.Text = "Loading content..."; 

        }


        async void NewsList_ItemRealized(object sender, ItemRealizationEventArgs e)
        {
            if (!((HomeViewModel)this.ViewModel).IsLoading 
                && this.NewsList.ItemsSource != null)
            {
                if (e.ItemKind == LongListSelectorItemKind.Item)
                {
                    if ((e.Container.Content as NewsItem).Equals(this.NewsList.ItemsSource[this.NewsList.ItemsSource.Count - 1]))
                    {
                        await ((HomeViewModel)this.ViewModel).LoadNews(20, this.skip += 20);
                    }
                }
            } 
        }

        async void TweetsList_ItemRealized(object sender, ItemRealizationEventArgs e)
        {

            if (!((HomeViewModel)this.ViewModel).IsLoading
                && this.TweetsList.ItemsSource != null)
            {
                if (e.ItemKind == LongListSelectorItemKind.Item)
                {
                    if ((e.Container.Content as TwitterItem).Equals(this.TweetsList.ItemsSource[this.TweetsList.ItemsSource.Count - 1]))
                    {
                        await((HomeViewModel)this.ViewModel).LoadTweets();
                    }
                }
            } 
        }

        private void NewsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var addedItem = e.AddedItems[0];
                if (addedItem is NewsItem)
                {
                    ((HomeViewModel)this.ViewModel).NewsItemSelected((NewsItem)addedItem);
                }
            }
        }

        private void TweetsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var addedItem = e.AddedItems[0];
                if (addedItem is TwitterItem)
                {
                    ((HomeViewModel)this.ViewModel).TwitterItemSelected((TwitterItem)addedItem);
                }
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            this.NewsList.SelectedItem = null;
            this.TweetsList.SelectedItem = null;
        }

        private async void ApplicationBarIconButton_Click(object sender, System.EventArgs e)
        {
            await ((HomeViewModel)this.ViewModel).RefreshAllData();
        }

        private void ApplicationBarMenuItem_Click(object sender, System.EventArgs e)
        {
            ((HomeViewModel)this.ViewModel).ShowPrivacyPolicy("http://myapppolicy.com/app/xboxone");
        }
    }
}