using System;
using MvvmCross.ViewModels;

namespace TodoApp.ViewModels
{
	public abstract class BaseViewModel : MvxViewModel
    {
		public string Title { get; protected set; }
    }

	public abstract class BaseViewModel<TParameter> : MvxViewModel<TParameter>
	{
		public string Title { get; protected set; }
	}
}
