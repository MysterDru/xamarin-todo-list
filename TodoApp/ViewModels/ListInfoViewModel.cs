using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MvvmCross.Commands;
using TodoApp.Models;
using TodoApp.Repositories;

namespace TodoApp.ViewModels
{
	public class ListInfoViewModel : BaseViewModel<ListInfoViewModel.Parameter>
	{
		ITodoRepository _repo;
		IUserDialogs _dialogs;
		Models.TodoList _list;

		public string ListTitle { get; private set; }
		public string ListDescription { get; private set; }

		public IMvxCommand SaveList => new MvxAsyncCommand(ExecuteSaveList);

		public ObservableCollection<Models.ToDoItem> Items { get; private set; }

		public IMvxCommand ShowOptions => new MvxAsyncCommand(ExecuteShowOptions);

		public IMvxCommand AddNewItem => new MvxAsyncCommand(ExecuteAddNewtItem);

		public IMvxCommand DeleteItem => new MvxAsyncCommand<Models.ToDoItem>(ExecuteDeleteItem);

		public ListInfoViewModel(
			ITodoRepository repo,
			IUserDialogs dialogs)
		{
			_repo = repo;
			_dialogs = dialogs;
		}

		public override async Task Initialize()
		{
			//return base.Initialize();
			Title = "List Info";

			ListTitle = _list.Title;
			ListDescription = _list.Description;

			var items = await _repo.GetItems(_list.Id);

			Items = new ObservableCollection<Models.ToDoItem>(items ?? new List<Models.ToDoItem>());
		}

		public override void Prepare(Parameter parameter)
		{
			_list = parameter.List;
		}

		async Task ExecuteSaveList()
		{
			if (string.IsNullOrEmpty(Title))
			{
				await _dialogs.AlertAsync("Title is required");
				return;
			}

			_list.Title = this.ListTitle;
			_list.Description = this.ListDescription;

			await _repo.UpdateList(_list.Id, _list);
		}

		async Task ExecuteAddNewtItem()
		{
			if (await CheckActive())
			{
				var result = await _dialogs.PromptAsync(
					message: "Name Task",
					title: "Add New Task");

				var newItemTitle = result.Value;

				var item = new Models.ToDoItem
				{
					Title = newItemTitle
				};

				item = await _repo.AddItem(_list.Id, item);

				Items.Add(item);
			}
		}

		private async Task ExecuteDeleteItem(ToDoItem arg)
		{
			if (await CheckActive())
			{
				await _repo.RemoveItem(_list.Id, arg.Id);

				Items.Remove(arg);
			}
		}

		async Task<bool> CheckActive()
		{
			if (!_list.IsActive)
			{
				await _dialogs.AlertAsync(
					message: "The list must be active in order to modify it.");
			}

			return _list.IsActive;
		}

		async Task ExecuteShowOptions()
		{
			bool isActive = _list.IsActive;
			var action = await _dialogs.ActionSheetAsync(
				"Options",
				"Cancel",
				"Delete",
				null,
				isActive ? "Deactivate" : "Activate");

			if (action == "Delete")
			{
				await DeleteList();
			}
			else if (action == "Deactivate")
			{
				await _repo.DeactivateList(_list.Id);
			}
			else if (action == "Activate")
			{
				await _repo.ActivateList(_list.Id);
			}
		}

		async Task DeleteList()
		{
			await this._repo.RemoveList(this._list.Id);

			await NavigationService.Close(this);
		}

		public class Parameter
		{
			public Models.TodoList List { get; set; }
		}
	}
}
