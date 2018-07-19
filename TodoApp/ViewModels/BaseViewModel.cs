using System;
using MvvmCross.ViewModels;

namespace TodoApp.ViewModels
{
	public class BaseViewModel : MvxViewModel
    {
		public string Title { get; protected set; }
    }
}
