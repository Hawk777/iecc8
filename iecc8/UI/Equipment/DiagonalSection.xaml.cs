using System.Windows;
using System.Windows.Media;

namespace Iecc8.UI.Equipment {
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
		public static readonly DependencyProperty TCBProperty = DependencyProperty.Register(nameof(TCB), typeof(bool), typeof(DiagonalSection), new PropertyMetadata(false, OnTCBPropertyChanged));

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

		/// <summary>
		/// Refreshes the shape of this object based on changes to its properties.
		/// </summary>
		private void UpdateShape() {
			Polygon.Data = (PathGeometry) FindResource(TCB ? "DiagonalSectionTCB" : "DiagonalSectionNoTCB");
		}

		/// <summary>
		/// Invoked when the TCB property changes on an object.
		/// </summary>
		/// <param name="d">The object whose value changed.</param>
		/// <param name="e">Details about the change.</param>
		private static void OnTCBPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			((DiagonalSection) d).UpdateShape();
		}
	}
}
