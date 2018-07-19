using System;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using TodoApp.ViewModels;

namespace TodoApp
{
	public class ToDoAppStart : MvvmCross.ViewModels.MvxAppStart
	{
		public ToDoAppStart(IMvxApplication application, IMvxNavigationService navigationService) : base(application, navigationService)
		{
		}

		protected override void NavigateToFirstViewModel(object hint = null)
		{
			this.NavigationService.Navigate<HomeViewModel>();
		}
	}
}
