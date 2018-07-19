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
	public partial class HomeView : MvxTableViewController
	{
		public HomeView(IntPtr handle)
			: base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var add = new UIBarButtonItem(UIBarButtonSystemItem.Add);
			this.NavigationItem.RightBarButtonItem = add;

			var source = new MvxStandardTableViewSource(this.TableView, "TitleText Title;");
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
			set.Bind(add)
			   .To(vm => vm.AddNewItem);
			set.Apply();
		}
	}
}
