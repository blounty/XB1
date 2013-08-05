using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Cirrious.MvvmCross.WindowsPhone.Views;
using System.Windows.Data;
using XboxOne.Core.ViewModels;
using XboxOne.Core.Models;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Bookmarks;

namespace XboxOne.WP8.Views
{
    public partial class GameView : MvxPhonePage
    {
        public GameView()
        {
            InitializeComponent();

            this.Loaded += GameView_Loaded;
            
        }

        void GameView_Loaded(object sender, RoutedEventArgs e)
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

        private void NewsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var addedItem = e.AddedItems[0];
                if (addedItem is NewsItem)
                {
                    ((GameViewModel)this.ViewModel).NewsItemSelected((NewsItem)addedItem);
                }
            }
        }

        private async void Refresh_Click(object sender, System.EventArgs e)
        {
            await ((GameViewModel)this.ViewModel).RefreshAllData();
        }

        private async void Pin_Click(object sender, EventArgs e)
        {
            await ((GameViewModel)this.ViewModel).AddSecondaryTile();
        }

        private void VideosList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var addedItem = e.AddedItems[0];
                if (addedItem is Video)
                {
                    ((GameViewModel)this.ViewModel).VideoSelected((Video)addedItem);
                }
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            this.NewsList.SelectedItem = null;
            this.VideosList.SelectedItem = null;
            FlurryWP8SDK.Api.LogEvent("Viewed Game", new List<FlurryWP8SDK.Models.Parameter> { new FlurryWP8SDK.Models.Parameter("GameName", ((GameViewModel)this.ViewModel).Game.Name) });
        }
    }
}