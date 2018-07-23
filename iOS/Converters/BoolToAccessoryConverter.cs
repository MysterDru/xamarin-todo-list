using System;
using System.Globalization;
using MvvmCross.Converters;
using UIKit;

namespace TodoApp.iOS.Converters
{
	internal class BoolToAccessoryConverter : MvxValueConverter<bool, UIKit.UITableViewCellAccessory>
    {
		protected override UITableViewCellAccessory Convert(bool value, Type targetType, object parameter, CultureInfo culture)
		{
			return value ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
		}
	}
}