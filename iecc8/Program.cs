using System;
using System.Windows;

namespace Iecc8 {
	public static class Program {
		[STAThread]
		public static void Main() {
			Application app = new Application();
			app.ShutdownMode = ShutdownMode.OnMainWindowClose;
			app.StartupUri = new Uri("UI/WelcomeWindow.xaml", UriKind.Relative);
			app.Run();
		}
	}
}
