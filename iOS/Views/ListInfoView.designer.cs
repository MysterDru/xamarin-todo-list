// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace TodoApp.iOS.Views
{
	[Register ("ListInfoView")]
	partial class ListInfoView
	{
		[Outlet]
		UIKit.UIBarButtonItem AddButton { get; set; }

		[Outlet]
		UIKit.UITextView DescrptionTextView { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }

		[Outlet]
		UIKit.UITextField TitleTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AddButton != null) {
				AddButton.Dispose ();
				AddButton = null;
			}

			if (DescrptionTextView != null) {
				DescrptionTextView.Dispose ();
				DescrptionTextView = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}

			if (TitleTextField != null) {
				TitleTextField.Dispose ();
				TitleTextField = null;
			}
		}
	}
}
