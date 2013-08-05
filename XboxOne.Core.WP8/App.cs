using Cirrious.CrossCore.IoC;
using Parse;

namespace XboxOne.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {

        public override void Initialize()
        {
            ParseClient.Initialize("5yU6Uf5QqT066zOB52KBZBAhf9qnrRPVlZrRzvp7", "pjPDnc7GZKERP3yiAw9sP3lXy7RRXwEaG4CGN8Qp");

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
            RegisterAppStart<ViewModels.HomeViewModel>();
        }
    }
}