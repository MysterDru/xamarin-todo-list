using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace TodoApp.iOS.Views
{
	[Register(nameof(TodoListViewCell))]
	public partial class TodoListViewCell : MvxTableViewCell
	{
		public TodoListViewCell(IntPtr handle)
			: base(handle)
		{
			this.DelayBind(() =>
			{
				var binding = this.CreateBindingSet<TodoListViewCell, Models.TodoItem>();

				binding
					.Bind(TextLabel)
					.To(vm => vm.Title);
				binding
					.Bind(DetailTextLabel)
					.To(vm => vm.Description);

				binding.Apply();
			});
		}
	}
}
