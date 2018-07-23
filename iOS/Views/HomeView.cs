using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using TodoApp.ViewModels;
using UIKit;

namespace TodoApp.iOS.Views
{
	[MvxFromStoryboard]
	[MvxRootPresentation(WrapInNavigationController = true)]
	public partial class HomeView : MvxViewController<HomeViewModel>
	{
		public HomeView(IntPtr handle)
			: base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, null);

			var source = new CustomDataSource(this, this.TableView, new NSString("TodoListViewCell"));
			TableView.Source = source;

			var set = this.CreateBindingSet<HomeView, ViewModels.HomeViewModel>();
			set.Bind(source)
			   .For(x => x.SelectionChangedCommand)
			   .To(vm => vm.SelectItem);
			set.Bind(source)
			   .For(x => x.ItemsSource)
			   .To(vm => vm.TodoLists);
			set.Bind(NavigationItem)
			   .For(x => x.Title)
			   .To(vm => vm.Title);
			set.Bind(AddButton)
			   .To(vm => vm.AddNewItem);
			set.Apply();
		}

		private class CustomDataSource : MvxStandardTableViewSource
		{
			HomeView viewController;
			public CustomDataSource(HomeView parentView, UITableView tableview, NSString identifier)
				: base(tableview, identifier)
			{
				viewController = parentView;
			}

			public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewRowAction deleteButton = UITableViewRowAction.Create(
				UITableViewRowActionStyle.Default,
				"Delete",
				delegate
				{
					var item = GetItemAt(indexPath) as Models.TodoList;
					viewController.ViewModel.DeleteList.Execute(item);

				});
				return new UITableViewRowAction[] { deleteButton };
			}
		}
	}
}
