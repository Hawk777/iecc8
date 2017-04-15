using System.Windows;
using System.Windows.Controls;

namespace Iecc8.UI {
	/// <summary>
	/// Interaction logic for NonTCDiagonalSection.xaml
	/// </summary>
	public partial class NonTCDiagonalSection : UserControl {
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
		public static readonly DependencyProperty TCBProperty = DependencyProperty.Register(nameof(TCB), typeof(bool), typeof(NonTCDiagonalSection));

		/// <summary>
		/// Constructs a new NonTCDiagonalSection.
		/// </summary>
		public NonTCDiagonalSection() {
			InitializeComponent();
		}
	}
}
