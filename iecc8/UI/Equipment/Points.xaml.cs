using Iecc8.UI.Common;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Iecc8.UI.Equipment {
	/// <summary>
	/// Interaction logic for Points.xaml
	/// </summary>
	public partial class Points : TrackSection {
		/// <summary>
		/// The ID number of the points.
		/// </summary>
		public short PointsID {
			get {
				return (short) GetValue(PointsIDProperty);
			}
			set {
				SetValue(PointsIDProperty, value);
			}
		}

		/// <summary>
		/// The points ID property.
		/// </summary>
		public static readonly DependencyProperty PointsIDProperty = DependencyProperty.Register(nameof(PointsID), typeof(short), typeof(Points));

		/// <summary>
		/// Whether the points are drawn in what would normally be the reverse position when they are actually normal and vice versa.
		/// </summary>
		public bool Invert {
			get {
				return (bool) GetValue(InvertProperty);
			}
			set {
				SetValue(InvertProperty, value);
			}
		}

		/// <summary>
		/// The invert property.
		/// </summary>
		public static readonly DependencyProperty InvertProperty = DependencyProperty.Register(nameof(Invert), typeof(bool), typeof(Points));

		/// <summary>
		/// Constructs a new Points.
		/// </summary>
		public Points() {
			InitializeComponent();
			Loaded += OnLoaded;
		}

		/// <summary>
		/// Which points this object displays.
		/// </summary>
		private World.Points PointsObject;

		/// <summary>
		/// Finishes initialization of this object.
		/// </summary>
		/// <param name="sender">This object.</param>
		/// <param name="e">Details of the event.</param>
		private void OnLoaded(object sender, EventArgs e) {
			MainViewModel vm = DataContext as MainViewModel;
			if (vm != null) {
				PointsObject = vm.World.Region.SubAreas[SubAreaID].PowerPoints[PointsID];
				PointsObject.PropertyChanged += OnPointsPropChanged;
				MouseDown += OnMouseDown;
			}
			Update();
		}

		/// <summary>
		/// Refreshes the appearance of the object.
		/// </summary>
		protected override void Update() {
			if (PointsObject != null) {
				if ((!PointsObject.Proved || PointsObject.Inconsistent) && !((MainViewModel) DataContext).BlinkClockSource.Value) {
					SetNoDraw();
				} else if (PointsObject.HandCrankable) {
					SetOutline();
				} else {
					SetFilled();
				}
				Polygon.Data = (PathGeometry) FindResource((PointsObject.Reversed ^ Invert) ? "PointsReverse" : "PointsNormal");
			}
		}

		protected override bool IsKeyedPoint {
			get {
				return PointsObject.Keyed;
			}
		}

		/// <summary>
		/// Sets up the polygon to draw filled in the proper colour.
		/// </summary>
		private void SetFilled() {
			Polygon.Fill = RenderColour;
			Polygon.Stroke = null;
		}

		/// <summary>
		/// Sets up the polygon to draw an outline in the proper colour.
		/// </summary>
		private void SetOutline() {
			Polygon.Fill = null;
			Polygon.Stroke = RenderColour;
		}

		/// <summary>
		/// Sets up the polygon to not draw at all.
		/// </summary>
		private void SetNoDraw() {
			Polygon.Fill = null;
			Polygon.Stroke = null;
		}

		/// <summary>
		/// Invoked when a property changes on the points.
		/// </summary>
		/// <param name="sender">The points.</param>
		/// <param name="e">Details of the change.</param>
		private void OnPointsPropChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(World.Points.Reversed) || e.PropertyName == nameof(World.Points.HandCrankable) || e.PropertyName == nameof(World.Points.Keyed)) {
				Update();
			} else if (e.PropertyName == nameof(World.Points.Proved) || e.PropertyName == nameof(World.Points.Inconsistent)) {
				MainViewModel vm = DataContext as MainViewModel;
				if (vm != null) {
					vm.BlinkClockSource.PropertyChanged -= OnBlinkChanged;
					if (!PointsObject.Proved || PointsObject.Inconsistent) {
						vm.BlinkClockSource.PropertyChanged += OnBlinkChanged;
					}
				}
				Update();
			}
		}

		/// <summary>
		/// Invoked when the blink clock changes value.
		/// </summary>
		/// <param name="sender">The blink clock.</param>
		/// <param name="e">Details of the change.</param>
		private void OnBlinkChanged(object sender, PropertyChangedEventArgs e) {
			Update();
		}

		/// <summary>
		/// Invoked when the user clicks on the points.
		/// </summary>
		/// <param name="sender">The points.</param>
		/// <param name="e">Details of the click event.</param>
		private async void OnMouseDown(object sender, MouseButtonEventArgs e) {
			if (e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Right) {
				if (PointsObject.Keyed) {
					PointsObject.Unkey();
				} else {
					bool reverse = e.ChangedButton == MouseButton.Right;
					if (PointsObject.Movable || (reverse == PointsObject.Reversed)) {
						await PointsObject.KeyAsync(reverse);
					}
				}
			} else if (e.ChangedButton == MouseButton.Middle) {
				if (PointsObject.HandCrankable) {
					PointsObject.HandCrankable = false;
				} else if (PointsObject.CheckHandCrankingAvailable()) {
					PointsObject.HandCrankable = true;
				}
			}
		}
	}
}
