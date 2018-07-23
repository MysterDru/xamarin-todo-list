using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		public ObservableCollection<Models.TodoList> TodoLists { get; private set; }

		public IMvxCommand AddNewItem => new MvxAsyncCommand(ExecuteAddNewtItem);

		public IMvxCommand SelectItem => new MvxAsyncCommand<Models.TodoList>(ExecuteSelectItem);

		public IMvxCommand DeleteList => new MvxAsyncCommand<Models.TodoList>(ExecuteDeleteItem);

		public HomeViewModel(IUserDialogs dialogs, ITodoRepository repo)
		{
			_dialogs = dialogs;
			_repo = repo;
		}

		public override async void ViewAppearing()
		{
			base.ViewAppearing();

			// for simplicity, just reload data whenever the view appears
			// handles when properites about the list change
			if (_isInitialized)
			{
				await LoadData();
			}
		}

		public override async Task Initialize()
		{
			await base.Initialize();

			this.Title = "To-Do List";

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

			TodoLists = new ObservableCollection<TodoList>(lists ?? new List<Models.TodoList>());
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

			_dialogs.ShowLoading(string.Empty);

			var created = await _repo.AddList(new Models.TodoList
			{
				Title = newItemTitle
			});

			// if user wants to make it active, activate this one,
			// calling the repo method will deactivate all other lists
			if(makeActive)
			{
				await _repo.ActivateList(created.Id);
			}

			TodoLists.Add(created);

			_dialogs.HideLoading();

			// navigate to the new list
			SelectItem.Execute(created);
		}

		private async Task ExecuteSelectItem(TodoList arg)
		{
			await this.NavigationService.Navigate<ListInfoViewModel, ListInfoViewModel.Parameter>(new ListInfoViewModel.Parameter
			{
				List = arg
			});
		}

		private async Task ExecuteDeleteItem(TodoList item)
		{
			bool confirm = await _dialogs.ConfirmAsync(
				"Are you sure you want to delete this list?",
				$"Delete {item.Title}",
				"Yes",
				"Cancel");

			if (confirm)
			{
				await _repo.RemoveList(item.Id);

				TodoLists.Remove(item);
			}
		}
	}
}
