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

namespace XboxOne.WP8.Views
{
    public partial class WebContentView : MvxPhonePage
    {

        public WebContentView()
        {
            InitializeComponent();

            this.Loaded += WebContentView_Loaded;

            this.WebBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.AdDisplay.Loaded += AdDisplay_Loaded;
            this.AdDisplay.ErrorOccurred += AdDisplay_ErrorOccurred;
            FlurryWP8SDK.Api.LogEvent("Viewed Content");
        }

        void AdDisplay_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            
        }

        void AdDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        void WebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            ((WebContentViewModel)this.ViewModel).IsLoading = false;
        }

        void WebContentView_Loaded(object sender, RoutedEventArgs e)
        {
            ((WebContentViewModel)this.ViewModel).IsLoading = true;

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
    }
}