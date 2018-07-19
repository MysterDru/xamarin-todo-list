
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using TodoApp.ViewModels;

namespace TodoApp
{
	public class App : MvxApplication
	{
		public override void Initialize()
		{
			CreatableTypes()
				.EndingWith("Service")
				.AsInterfaces()
				.RegisterAsLazySingleton();

			Mvx.RegisterSingleton(() => UserDialogs.Instance);

			RegisterCustomAppStart<ToDoAppStart>();
		}
	}
}
