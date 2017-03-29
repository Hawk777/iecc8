using System.Windows.Controls;

namespace Iecc8.UI {
	/// <summary>
	/// Interaction logic for RadioTransmitBar.xaml
	/// </summary>
	public partial class RadioTransmitBar : UserControl {
		public RadioTransmitBar() {
			InitializeComponent();
		}

		public void FocusMessageBox() {
			ChannelBox.SelectAll();
			MessageBox.Focus();
		}

		private void HideAfterSending(object sender, System.Windows.RoutedEventArgs e) {
			Visibility = System.Windows.Visibility.Collapsed;
		}
	}
}
