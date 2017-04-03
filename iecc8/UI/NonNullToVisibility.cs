using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Iecc8.UI {
	/// <summary>
	/// Converts an object's non-nullness into a visibility.
	/// </summary>
	public class NonNullToVisibility : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return (value != null) ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
