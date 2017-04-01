using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace Iecc8.UI {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow(MainViewModel vm) {
			DataContext = vm;
			Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
			InitializeComponent();
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			base.OnKeyDown(e);
			if (e.Key == Key.F2) {
				MainViewModel vm = (MainViewModel) DataContext;
				vm.ShowTrainList = !vm.ShowTrainList;
			} else if (e.Key == Key.F3) {
				ShowHideChannelGrid(this, e);
			} else if (e.Key == Key.F4) {
				ShowHideRadioTransmitBar(this, e);
			}
		}

		private void ShowHideRadioTransmitBar(object sender, RoutedEventArgs e) {
			if (RadioTransmitter.Visibility == Visibility.Visible) {
				RadioTransmitter.Visibility = Visibility.Collapsed;
			} else {
				RadioTransmitter.Visibility = Visibility.Visible;
				RadioTransmitter.FocusMessageBox();
			}
		}

		private void ShowHideChannelGrid(object sender, RoutedEventArgs e) {
			MainViewModel vm = (MainViewModel) DataContext;
			vm.ShowChannelGrid = !vm.ShowChannelGrid;
		}

		private void MessageListPreviewMouseDown(object sender, MouseButtonEventArgs e) {
			ListBox lb = (ListBox) sender;
			ListBoxItem item = (ListBoxItem) ItemsControl.ContainerFromElement(lb, (DependencyObject) e.OriginalSource);
			if (item != null) {
				int index = lb.ItemContainerGenerator.IndexFromContainer(item);
				((MainViewModel) DataContext).ClearMessage(index);
			}
		}

		private void OnTrainListButtonClick(object sender, RoutedEventArgs e) {
			MainViewModel vm = (MainViewModel) DataContext;
			vm.ShowTrainList = !vm.ShowTrainList;
		}
	}
}
