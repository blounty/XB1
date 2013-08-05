using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Parse;

namespace XboxOne.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {

        public override void Initialize()
        {
            ParseClient.Initialize("","");

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
            RegisterAppStart<ViewModels.HomeViewModel>();
        }
    }
}