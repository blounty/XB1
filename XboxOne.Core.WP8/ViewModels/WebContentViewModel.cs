using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XboxOne.Core.ViewModels
{
    public class WebContentViewModel
        :MvxViewModel
    {
        private string contentUrl;

        public string ContentUrl
        {
            get { return contentUrl; }
            set
            {
                contentUrl = value;
                RaisePropertyChanged(() => this.ContentUrl);
            }
        }


        private bool isLoading;
        public bool IsLoading 
        { 
            get 
            { 
                return this.isLoading; 
            } 
            set 
            { 
                this.isLoading = value;
                RaisePropertyChanged(() => this.IsLoading); 
            } 
        }

        public void Init(string contentUrl)
        {
            this.ContentUrl = contentUrl;
        }

    }
}
