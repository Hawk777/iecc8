using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace Iecc8.UI {
	/// <summary>
	/// Interaction logic for WelcomeWindow.xaml
	/// </summary>
	public partial class WelcomeWindow : Window {
		public WelcomeWindow() {
			Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
			InitializeComponent();
		}
	}
}
