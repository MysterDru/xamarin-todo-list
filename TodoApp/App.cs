
using Acr.UserDialogs;
using MonkeyCache;
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
			MonkeyCache.FileStore.Barrel.ApplicationId = "Todoapp";

			CreatableTypes()
				.EndingWith("Service")
				.AsInterfaces()
				.RegisterAsLazySingleton();

			CreatableTypes()
				.EndingWith("Repository")
				.AsInterfaces()
				.RegisterAsLazySingleton();

			Mvx.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);
			Mvx.RegisterSingleton<IBarrel>(() => MonkeyCache.FileStore.Barrel.Current);

			RegisterCustomAppStart<ToDoAppStart>();
		}
	}
}
