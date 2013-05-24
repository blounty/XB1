using Cirrious.CrossCore.IoC;
using Microsoft.WindowsAzure.MobileServices;

namespace XboxOne.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public static MobileServiceClient MobileService = new MobileServiceClient(
            "https://xboxone.azure-mobile.net/",
            "KHxrrhkbhUjfSjPnhxDZVkmpmXYLlN50"
            );

        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
            RegisterAppStart<ViewModels.FirstViewModel>();
        }
    }
}