using Iecc8.Messages;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Iecc8.UI {
	/// <summary>
	/// Converts an EEngineerType to a boolean value indicating whether the engineer is an AI.
	/// </summary>
	public class EngineerTypeToIsAI : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return ((EEngineerType) value) == EEngineerType.AI;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return ((bool) value) ? EEngineerType.AI : EEngineerType.None;
		}
	}
}
