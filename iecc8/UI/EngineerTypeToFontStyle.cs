using Iecc8.Messages;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Iecc8.UI {
	/// <summary>
	/// Converter from engineer type values to font styles (italic for AI, plain for other).
	/// </summary>
	public class EngineerTypeToFontStyle : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return ((EEngineerType) value) == EEngineerType.AI ? FontStyles.Italic : FontStyles.Normal;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return Binding.DoNothing;
		}
	}
}
