using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Iecc8.UI {
	/// <summary>
	/// Converter from type values to text colours.
	/// </summary>
	public class MessageTypeToColour : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			switch ((Message.EType) value) {
				case Message.EType.DTMF:
					return new SolidColorBrush(Colors.Yellow);
				case Message.EType.DTMFEmergency:
					return new SolidColorBrush(Colors.Red);
				case Message.EType.Radio:
					return new SolidColorBrush(Colors.White);
				case Message.EType.Miscellaneous:
					return new SolidColorBrush(Colors.Cyan);
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return Binding.DoNothing;
		}
	}
}
