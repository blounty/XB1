﻿#pragma checksum "C:\Projects\XboxOne\XboxOne.WP8\Views\YouTubeContentView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5190320691EEBD803BCBB2AB78164198"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.PlayerFramework;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace XboxOne.WP8.Views {
    
    
    public partial class YouTubeContentView : Cirrious.MvvmCross.WindowsPhone.Views.MvxPhonePage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.PlayerFramework.MediaPlayer VideoPlayer;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/XboxOne.WP8;component/Views/YouTubeContentView.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.VideoPlayer = ((Microsoft.PlayerFramework.MediaPlayer)(this.FindName("VideoPlayer")));
        }
    }
}

