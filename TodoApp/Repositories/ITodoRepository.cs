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

			barrel.Add("todoList", list, TimeSpan.FromDays(600));

			return newList;
		}
	}
}
