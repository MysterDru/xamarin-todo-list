using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MonkeyCache;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using TodoApp.Models;
using TodoApp.Repositories;

namespace TodoApp.ViewModels
{
	public class HomeViewModel : BaseViewModel
	{
		readonly IUserDialogs _dialogs;
		readonly ITodoRepository _repo;

		public IEnumerable<Models.TodoList> TodoLists { get; private set; }

		public IMvxCommand AddNewItem => new MvxAsyncCommand(ExecuteAddNewtItem);

		public IMvxCommand SelectItem => new MvxAsyncCommand<Models.TodoList>(ExecuteSelectItem);

		public HomeViewModel(IUserDialogs dialogs, ITodoRepository repo)
		{
			_dialogs = dialogs;
			_repo = repo;
		}

		public override async Task Initialize()
		{
			this.Title = "Drew's To-Do";

			var lists = await _repo.GetLists();

			TodoLists = lists ?? new List<Models.TodoList>();

			if (!TodoLists.Any())
			{
				AddNewItem.Execute();
			}
		}

		private async Task ExecuteAddNewtItem()
		{
			var result = await _dialogs.PromptAsync(
				message: "Name of ToDo Item",
				title: "Add New Item");

			var newItemTitle = result.Value;

			var created = await _repo.AddList(new Models.TodoList { Title = newItemTitle });

			var copy = TodoLists.ToList();
			copy.Add(created);

			TodoLists = copy;
		}

		private async Task ExecuteSelectItem(TodoList arg)
		{
			this.NavigationService.Navigate<ListInfoViewModel, ListInfoViewModel.Parameter>(new ListInfoViewModel.Parameter
			{
				List = arg
			});
		}
	}
}
