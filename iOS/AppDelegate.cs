using Foundation;
using MvvmCross.Platforms.Ios.Core;
using UIKit;

namespace TodoApp.iOS
{
	[Register("AppDelegate")]
	public class AppDelegate : MvxApplicationDelegate<MvxIosSetup<App>, App>
	{
	}
}

