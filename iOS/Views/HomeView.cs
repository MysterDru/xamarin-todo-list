using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
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

			var set = this.CreateBindingSet<HomeView, ViewModels.HomeViewModel>();
			set.Bind(NavigationItem)
			   .For(x => x.Title)
			   .To(vm => vm.Title);
			set.Bind(add)
			   .To(vm => vm.AddNewItem);
			set.Apply();
		}
	}
}
