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
			}
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
