using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using TodoApp.iOS.Converters;

namespace TodoApp.iOS.Views
{
	[Register(nameof(TodoItemViewCell))]
	public partial class TodoItemViewCell : MvxTableViewCell
	{
		public TodoItemViewCell(IntPtr handle)
			: base(handle)
		{
			this.DelayBind(() =>
			{
				var binding = this.CreateBindingSet<TodoItemViewCell, Models.TodoItem>();

				binding
					.Bind(TextLabel)
					.To(vm => vm.Title);
				binding
					.Bind(DetailTextLabel)
					.To(vm => vm.Description);

				binding.Bind(this)
					   .For(x => x.Accessory)
					   .To(vm => vm.IsCompleted)
					   .WithConversion(new BoolToAccessoryConverter());

				binding.Apply();
			});
		}
	}
}
