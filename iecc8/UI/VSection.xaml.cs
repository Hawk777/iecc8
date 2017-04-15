using System.Windows;

namespace Iecc8.UI {
	/// <summary>
	/// Interaction logic for VSection.xaml
	/// </summary>
	public partial class VSection : TrackSection {
		/// <summary>
		/// Constructs a new VSection.
		/// </summary>
		public VSection() {
			InitializeComponent();
		}

		/// <summary>
		/// Refreshes the appearance of this object.
		/// </summary>
		protected override void Update() {
			Rectangle.Fill = RenderColour;
		}
	}
}
