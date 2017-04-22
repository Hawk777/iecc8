using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Iecc8.UI.Common {
	/// <summary>
	/// Converter from boolean current-location values to font styles (plain for current, italic for old).
	/// </summary>
	public class LocationCurrentToFontStyle : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return (bool) value ? FontStyles.Normal : FontStyles.Italic;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return Binding.DoNothing;
		}
	}
}
