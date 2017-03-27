using System;

namespace Iecc8.UI {
	/// <summary>
	/// Interaction logic for ExitTriangle.xaml
	/// </summary>
	public partial class ExitTriangle : SignalBase {
		/// <summary>
		/// Constructs an ExitTriangle.
		/// </summary>
		public ExitTriangle() {
			InitializeComponent();
		}

		/// <summary>
		/// Does nothing, as an exit triangle is entirely static.
		/// </summary>
		protected override void InitUI() {
		}
	}
}
