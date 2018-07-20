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

		bool _isInitialized = false;

		public IEnumerable<Models.TodoList> TodoLists { get; private set; }

		public IMvxCommand AddNewItem => new MvxAsyncCommand(ExecuteAddNewtItem);

		public IMvxCommand SelectItem => new MvxAsyncCommand<Models.TodoList>(ExecuteSelectItem);

		public HomeViewModel(IUserDialogs dialogs, ITodoRepository repo)
		{
			_dialogs = dialogs;
			_repo = repo;
		}

		public override async void ViewAppearing()
		{
			base.ViewAppearing();

			// reload data in the event "IsActive" changed
			// or a list was deleted
			if (_isInitialized)
			{
				await LoadData();
			}
		}

		public override async Task Initialize()
		{
			this.Title = "Drew's To-Do";

			await LoadData();

			if (!TodoLists.Any())
			{
				AddNewItem.Execute();
			}

			_isInitialized = true;
		}

		async Task LoadData()
		{
			var lists = await _repo.GetLists();

			TodoLists = lists ?? new List<Models.TodoList>();
		}

		private async Task ExecuteAddNewtItem()
		{
			var result = await _dialogs.PromptAsync(
				message: "Name of ToDo Item",
				title: "Add New Item");

			var newItemTitle = result.Value;

			bool makeActive = true;
			if (this.TodoLists.Any(x => x.IsActive))
			{
				makeActive = await _dialogs.ConfirmAsync(
					$"Do you want to make list '{result.Value}' your active list?",
					"Active List",
					okText: "Yes",
					cancelText: "No");
			}

			var created = await _repo.AddList(new Models.TodoList
			{
				Title = newItemTitle,
				IsActive = makeActive
			});

			var copy = TodoLists.ToList();
			copy.Add(created);

			TodoLists = copy;
		}

		private async Task ExecuteSelectItem(TodoList arg)
		{
			await this.NavigationService.Navigate<ListInfoViewModel, ListInfoViewModel.Parameter>(new ListInfoViewModel.Parameter
			{
				List = arg
			});
		}
	}
}
