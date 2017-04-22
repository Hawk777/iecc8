using Iecc8.World;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace Iecc8.UI {
	/// <summary>
	/// Interaction logic for SignalStem.xaml
	/// </summary>
	public partial class SignalStem : UserControl {
		/// <summary>
		/// Constructs a new SignalStem.
		/// </summary>
		public SignalStem() {
			InitializeComponent();
		}

		/// <summary>
		/// Initializes this stem.
		/// </summary>
		/// <param name="signal">The signal.</param>
		public void Init(World.Signal signal) {
			ControlledSignal cs = signal as ControlledSignal;
			if (cs != null) {
				signal.PropertyChanged += OnSignalPropChanged;
				StemPath.Data = (PathGeometry) FindResource("SignalStemControlled");
			} else {
				StemPath.Data = (PathGeometry) FindResource("SignalStemAutomatic");
			}
			UpdateRectangles(signal);
		}

		/// <summary>
		/// Updates the colour of the stem and presence or absence of the automatic crossbar.
		/// </summary>
		/// <param name="signal">The signal.</param>
		private void UpdateRectangles(World.Signal signal) {
			ControlledSignal cs = signal as ControlledSignal;
			bool hasRoute;
			if (cs != null) {
				hasRoute = cs.CurrentRoute != null;
			} else {
				hasRoute = false;
			}
			Brush brush = (Brush) FindResource(hasRoute ? "LockedWhite" : "IdleGrey");
			StemPath.Fill = brush;
		}

		/// <summary>
		/// Invoked when some property of the signal changes.
		/// </summary>
		/// <param name="sender">The signal.</param>
		/// <param name="e">Details of the change.</param>
		private void OnSignalPropChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(World.ControlledSignal.CurrentRoute)) {
				UpdateRectangles((World.Signal) sender);
			}
		}
	}
}
