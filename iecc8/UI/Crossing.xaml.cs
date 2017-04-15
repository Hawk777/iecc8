using Iecc8.World;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Iecc8.UI {
	/// <summary>
	/// Interaction logic for Crossing.xaml
	/// </summary>
	public partial class Crossing : UserControl {
		/// <summary>
		/// Which sub-area this crossing is in.
		/// </summary>
		public ushort SubAreaID {
			get {
				return (ushort) GetValue(SubAreaIDProperty);
			}
			set {
				SetValue(SubAreaIDProperty, value);
			}
		}

		/// <summary>
		/// The sub-area ID property.
		/// </summary>
		public static readonly DependencyProperty SubAreaIDProperty = DependencyProperty.Register(nameof(SubAreaID), typeof(ushort), typeof(Crossing));

		/// <summary>
		/// The track circuit that the normal part of this crossing is part of.
		/// </summary>
		public int NormalTrackCircuitID {
			get {
				return (int) GetValue(NormalTrackCircuitIDProperty);
			}
			set {
				SetValue(NormalTrackCircuitIDProperty, value);
			}
		}

		/// <summary>
		/// The normal track circuit ID property.
		/// </summary>
		public static readonly DependencyProperty NormalTrackCircuitIDProperty = DependencyProperty.Register(nameof(NormalTrackCircuitID), typeof(int), typeof(Crossing));

		/// <summary>
		/// The track circuit that the reverse part of this crossing is part of.
		/// </summary>
		public int ReverseTrackCircuitID {
			get {
				return (int) GetValue(ReverseTrackCircuitIDProperty);
			}
			set {
				SetValue(ReverseTrackCircuitIDProperty, value);
			}
		}

		/// <summary>
		/// The reverse track circuit ID property.
		/// </summary>
		public static readonly DependencyProperty ReverseTrackCircuitIDProperty = DependencyProperty.Register(nameof(ReverseTrackCircuitID), typeof(int), typeof(Crossing));

		/// <summary>
		/// Constructs a new Crossing.
		/// </summary>
		public Crossing() {
			Loaded += OnLoaded;
			RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
			InitializeComponent();
		}

		/// <summary>
		/// Updates the graphical appearance of this object.
		/// </summary>
		private void Update() {
			string key;
			if ((NormalTrackCircuit != null) && (ReverseTrackCircuit != null)) {
				if (NormalTrackCircuit.RouteLocked) {
					key = "PointsNormal";
				} else if (ReverseTrackCircuit.RouteLocked) {
					key = "PointsReverse";
				} else {
					key = "PointsBoth";
				}
			} else {
				key = "PointsBoth";
			}
			((PathGeometry) Polygon.Data).Figures = (PathFigureCollection) FindResource(key);
			Polygon.Fill = RenderColour;
		}

		/// <summary>
		/// The colour in which this section should be rendered, based on the states of the track circuits.
		/// </summary>
		private Brush RenderColour {
			get {
				MainViewModel vm = DataContext as MainViewModel;
				if ((NormalTrackCircuit == null) || (ReverseTrackCircuit == null)) {
					return (Brush) FindResource("IdleGrey");
				} else if (NormalTrackCircuit.Occupied || ReverseTrackCircuit.Occupied) {
					return (Brush) FindResource("OccupiedRed");
				} else if (NormalTrackCircuit.RouteLocked || ReverseTrackCircuit.RouteLocked) {
					return (Brush) FindResource("LockedWhite");
				} else {
					return (Brush) FindResource("IdleGrey");
				}
			}
		}

		/// <summary>
		/// The track circuit that the normal part of this section is part of.
		/// </summary>
		private TrackCircuit NormalTrackCircuit;

		/// <summary>
		/// The track circuit that the reverse part of this section is part of.
		/// </summary>
		private TrackCircuit ReverseTrackCircuit;

		/// <summary>
		/// Finishes initialization of this object.
		/// </summary>
		/// <param name="sender">This object.</param>
		/// <param name="e">Details of the event.</param>
		private void OnLoaded(object sender, EventArgs e) {
			MainViewModel vm = DataContext as MainViewModel;
			if (vm != null) {
				NormalTrackCircuit = vm.World.Region.SubAreas[SubAreaID].TrackCircuits[NormalTrackCircuitID];
				NormalTrackCircuit.PropertyChanged += OnTCPropChanged;
				ReverseTrackCircuit = vm.World.Region.SubAreas[SubAreaID].TrackCircuits[ReverseTrackCircuitID];
				ReverseTrackCircuit.PropertyChanged += OnTCPropChanged;
			}
			Update();
		}

		/// <summary>
		/// Invoked when a property changes on the track circuit.
		/// </summary>
		/// <param name="sender">The track circuit.</param>
		/// <param name="e">Information about the change.</param>
		private void OnTCPropChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(TrackCircuit.Occupied) || e.PropertyName == nameof(TrackCircuit.RouteLocked)) {
				Update();
			}
		}
	}
}
