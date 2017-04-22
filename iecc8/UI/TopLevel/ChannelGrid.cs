using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Iecc8.UI.TopLevel {
	/// <summary>
	/// A grid of checkboxes to subscribe to or unsubscribe from radio channels.
	/// </summary>
	public class ChannelGrid : Grid {
		/// <summary>
		/// Constructs a new ChannelGrid.
		/// </summary>
		public ChannelGrid() {
			// Make the grid rows and columns.
			for (uint i = 0; i != 10; ++i) {
				RowDefinition rd = new RowDefinition();
				rd.Height = new GridLength(1.0, GridUnitType.Star);
				RowDefinitions.Add(rd);
				ColumnDefinition cd = new ColumnDefinition();
				cd.Width = new GridLength(1.0, GridUnitType.Star);
				ColumnDefinitions.Add(cd);
			}

			// Make the checkboxes.
			for (uint row = 0; row != 10; ++row) {
				for (uint col = 0; col != 10; ++col) {
					uint index = row * 10 + col;
					CheckBox cb = new CheckBox();
					if (index == 0) {
						cb.IsEnabled = false;
						cb.IsChecked = true;
					}
					cb.Margin = new Thickness(2.0);
					cb.Content = index;
					SetRow(cb, (int) row);
					SetColumn(cb, (int) col);
					Binding b = new Binding("[" + index + "]");
					b.Mode = BindingMode.TwoWay;
					cb.SetBinding(CheckBox.IsCheckedProperty, b);
					Children.Add(cb);
				}
			}
		}
	}
}
