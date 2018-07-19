using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using TodoApp.Repositories;

namespace TodoApp.ViewModels
{
	public class ListInfoViewModel : BaseViewModel<ListInfoViewModel.Parameter>
	{
		ITodoRepository _repo;
		Models.TodoList _list;

		public IMvxCommand AddNewItem => new MvxAsyncCommand(ExecuteAddNewtItem);

		public ListInfoViewModel(ITodoRepository repo)
		{
			_repo = repo;
		}

		public override Task Initialize()
		{
			//return base.Initialize();
			Title = _list.Title;

			return base.Initialize();
		}

		public override void Prepare(Parameter parameter)
		{
			_list = parameter.List;
		}

		Task ExecuteAddNewtItem()
		{
			throw new NotImplementedException();
		}

		public class Parameter
		{
			public Models.TodoList List { get; set; }
		}
	}
}
