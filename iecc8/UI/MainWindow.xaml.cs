using System.Globalization;
using System.Windows;
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
	}
}
