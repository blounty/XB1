using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Plugins.Bookmarks;
using Cirrious.MvvmCross.Plugins.Bookmarks.WindowsPhone;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;

namespace XboxOne.WP8
{
    public class Setup : MvxPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame) : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
        protected override void PerformBootstrapActions()
        {
            base.PerformBootstrapActions();

            Mvx.RegisterSingleton<IMvxBookmarkLibrarian>(new MvxWindowsPhoneLiveTileBookmarkLibrarian());
        }

    }
}