using System.Windows;

namespace Iecc8.UI {
	/// <summary>
	/// Interaction logic for DiagonalSection.xaml
	/// </summary>
	public partial class DiagonalSection : TrackSection {
		/// <summary>
		/// Whether or not there is a track-circuit break on the right side of this section.
		/// </summary>
		public bool TCB {
			get {
				return (bool) GetValue(TCBProperty);
			}
			set {
				SetValue(TCBProperty, value);
			}
		}

		/// <summary>
		/// The track-circuit break property.
		/// </summary>
		public static readonly DependencyProperty TCBProperty = DependencyProperty.Register(nameof(TCB), typeof(bool), typeof(DiagonalSection));

		/// <summary>
		/// Constructs a new DiagonalSection.
		/// </summary>
		public DiagonalSection() {
			InitializeComponent();
		}

		/// <summary>
		/// Finishes initialization of this object.
		/// </summary>
		public override void EndInit() {
			base.EndInit();
			Update();
		}

		/// <summary>
		/// Refreshes the appearance of this object.
		/// </summary>
		protected override void Update() {
			Polygon.Fill = RenderColour;
		}
	}
}
