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

namespace XboxOne.WP8.Views
{
    public partial class YouTubeContentView : MvxPhonePage
    {
        public YouTubeContentView()
        {
            InitializeComponent();

            this.Loaded += YouTubeContentView_Loaded;
            this.AdDisplay.Loaded += AdDisplay_Loaded;
            this.AdDisplay.ErrorOccurred += AdDisplay_ErrorOccurred;
            FlurryWP8SDK.Api.LogEvent("Played YouTube Video");
        }

        void YouTubeContentView_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        void AdDisplay_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {

        }

        void AdDisplay_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}