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

		public ObservableCollection<Models.TodoItem> Items { get; private set; }

		public IMvxCommand ShowOptions => new MvxAsyncCommand(ExecuteShowOptions);

		public IMvxCommand AddNewItem => new MvxAsyncCommand(ExecuteAddNewtItem);

		public IMvxCommand CompleteItem => new MvxAsyncCommand<Models.TodoItem>(ExecuteCompleteItem);

		public IMvxCommand DeleteItem => new MvxAsyncCommand<Models.TodoItem>(ExecuteDeleteItem);

		public ListInfoViewModel(
			ITodoRepository repo,
			IUserDialogs dialogs)
		{
			_repo = repo;
			_dialogs = dialogs;
		}

		public override async Task Initialize()
		{
			await base.Initialize();

			Title = "List Info";

			ListTitle = _list.Title;
			ListDescription = _list.Description;

			var items = await _repo.GetItems(_list.Id);

			Items = new ObservableCollection<Models.TodoItem>(items ?? new List<Models.TodoItem>());
		}

		public override void Prepare(Parameter parameter)
		{
			_list = parameter.List;
		}

		async Task ExecuteCompleteItem(Models.TodoItem item)
		{
			item.IsCompleted = true;
			item.CompletedOn = DateTime.UtcNow;

			await _repo.UpdateItem(_list.Id, item.Id, item);

			var orderd = Items.OrderBy(x => x.CompletedOn);

			this.Items = new ObservableCollection<TodoItem>(orderd);
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
			if (await CheckIfListIsActive())
			{
				var result = await _dialogs.PromptAsync(
					message: "Name Task",
					title: "Add New Task");

				var newItemTitle = result.Value;

				var item = new Models.TodoItem
				{
					Title = newItemTitle
				};

				item = await _repo.AddItem(_list.Id, item);

				Items.Add(item);
			}
		}

		async Task ExecuteDeleteItem(TodoItem arg)
		{
			bool confirm = await _dialogs.ConfirmAsync(
				"Are you sure you want to delete this item?",
				$"Delete {arg.Title}",
				"Yes",
				"Cancel");

			if (confirm)
			{
				if (await CheckIfListIsActive())
				{
					await _repo.RemoveItem(_list.Id, arg.Id);

					Items.Remove(arg);
				}
			}
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
			bool confirm = await _dialogs.ConfirmAsync(
				"Are you sure you want to delete this list?",
				$"Delete {_list.Title}",
				"Yes",
				"Cancel");

			if (confirm && await CheckIfListIsActive())
			{
				await this._repo.RemoveList(this._list.Id);

				await NavigationService.Close(this);
			}
		}

		async Task<bool> CheckIfListIsActive()
		{
			if (!_list.IsActive)
			{
				await _dialogs.AlertAsync(
					message: "The list must be active in order to modify it.");
			}

			return _list.IsActive;
		}

		public class Parameter
		{
			public Models.TodoList List { get; set; }
		}
	}
}
