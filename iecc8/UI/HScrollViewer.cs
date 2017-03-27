using System.Windows.Controls;
using System.Windows.Input;

namespace Iecc8.UI {
	/// <summary>
	/// A scroll viewer that only scrolls horizontally, and does so using the mouse wheel.
	/// </summary>
	public class HScrollViewer : ScrollViewer {
		/// <summary>
		/// Constructs a new HScrollViewer.
		/// </summary>
		public HScrollViewer() {
			HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
			VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
		}

		/// <summary>
		/// Handles a scroll of the mouse wheel.
		/// </summary>
		/// <param name="e">Details of the event.</param>
		protected override void OnPreviewMouseWheel(MouseWheelEventArgs e) {
			base.OnPreviewMouseWheel(e);
			if (e.Delta < 0) {
				for (int i = 0; i >= e.Delta; i -= 30) {
					LineRight();
				}
			} else {
				for (int i = 0; i <= e.Delta; i += 30) {
					LineLeft();
				}
			}
		}
	}
}
