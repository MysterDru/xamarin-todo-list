using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonkeyCache;
using TodoApp.Models;

namespace TodoApp.Repositories
{
	public interface ITodoRepository
	{
		Task<IEnumerable<TodoList>> GetLists();

		Task<TodoList> AddList(TodoList list);

		Task RemoveList(Guid id);

		Task<IEnumerable<ToDoItem>> GetItems(Guid listId);

		Task<ToDoItem> AddItem(Guid listId, ToDoItem item);

		Task RemoveItem(Guid listId, Guid itemId);
	}

	public class TodoRepository : ITodoRepository
	{
		IBarrel barrel;

		public TodoRepository(IBarrel barrel)
		{
			this.barrel = barrel;
		}

		public async Task<IEnumerable<TodoList>> GetLists()
		{
			IEnumerable<TodoList> lists = null;

			await Task.Run(() =>
			{
				lists = barrel.Get<IEnumerable<Models.TodoList>>("todoLists");
			});

			return lists ?? new List<TodoList>();
		}

		public async Task<TodoList> AddList(TodoList newList)
		{
			newList.Id = Guid.NewGuid();

			var list = (await GetLists()).ToList();
			list.Add(newList);

			barrel.Add("todoLists", list, TimeSpan.FromDays(600));

			return newList;
		}

		public async Task RemoveList(Guid id)
		{
			var list = (await GetLists()).ToList();
			var found = list.FirstOrDefault(x => x.Id == id);

			list.Remove(found);

			barrel.Add("todoLists", list, TimeSpan.FromDays(600));
		}

		public async Task<IEnumerable<ToDoItem>> GetItems(Guid listId)
		{
			return await Task.Run(() =>
			{
				string listKey = listId.ToString().Replace("-", string.Empty);
				var values = barrel.Get<IEnumerable<ToDoItem>>($"ToDoItem{listKey}");

				return values;
			});
		}

		public async Task<ToDoItem> AddItem(Guid listId, ToDoItem item)
		{
			var items = (await GetItems(listId))?.ToList() ?? new List<ToDoItem>();

			item.Id = Guid.NewGuid();
			items.Add(item);

			string listKey = listId.ToString().Replace("-", string.Empty);
			barrel.Add($"ToDoItem{listKey}", items, TimeSpan.FromDays(600));

			return item;
		}

		public async Task RemoveItem(Guid listId, Guid itemId)
		{
			var items = (await GetItems(itemId))?.ToList() ?? new List<ToDoItem>();

			var item = items.FirstOrDefault(x => x.Id == itemId);

			items.Remove(item);

			string listKey = listId.ToString().Replace("-", string.Empty);
			barrel.Add($"ToDoItem{listKey}", items, TimeSpan.FromDays(600));
		}
	}
}
