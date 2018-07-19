using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace TodoApp.ViewModels
{
	public class HomeViewModel : BaseViewModel
	{
		readonly IUserDialogs _dialogs;

		public IMvxCommand AddNewItem => new MvxAsyncCommand(ExecuteAddNewtItem);

		public HomeViewModel(IUserDialogs dialogs)
		{
			_dialogs = dialogs;
		}

		//public override Task Initialize()
		//{
		//	this.Title = "Drew's To-Do";
		//	return base.Initialize();
		//}

		private async Task ExecuteAddNewtItem()
		{
			var result = await _dialogs.PromptAsync(
				message: "Name of ToDo Item",
				title: "Add New Item");

			var newItemTitle = result.Value;
		}
	}
}
