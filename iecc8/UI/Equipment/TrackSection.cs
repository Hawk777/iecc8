using Iecc8.UI.Common;
using Iecc8.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Iecc8.UI.Equipment {
	/// <summary>
	/// A section of track.
	/// </summary>
	/// <remarks>
	/// This class provides properties that are common to all track sections and supporting functions, but leaves subclasses to define the exact shape of the section.
	/// </remarks>
	public abstract class TrackSection : UserControl {
		/// <summary>
		/// Which sub-area this section is in.
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
		public static readonly DependencyProperty SubAreaIDProperty = DependencyProperty.Register(nameof(SubAreaID), typeof(ushort), typeof(TrackSection));

		/// <summary>
		/// The track circuit that this section is part of.
		/// </summary>
		public int TrackCircuitID {
			get {
				return (int) GetValue(TrackCircuitIDProperty);
			}
			set {
				SetValue(TrackCircuitIDProperty, value);
			}
		}

		/// <summary>
		/// The track circuit ID property.
		/// </summary>
		public static readonly DependencyProperty TrackCircuitIDProperty = DependencyProperty.Register(nameof(TrackCircuitID), typeof(int), typeof(TrackSection));

		/// <summary>
		/// The list of points and the positions they must be in in order for this section to be part of the current route.
		/// </summary>
		public List<SectionPointPosition> PointPositions {
			get {
				return (List<SectionPointPosition>) GetValue(PointPositionsProperty);
			}
			set {
				SetValue(PointPositionsProperty, value);
			}
		}

		/// <summary>
		/// The point positions property.
		/// </summary>
		public static readonly DependencyProperty PointPositionsProperty = DependencyProperty.Register(nameof(PointPositions), typeof(List<SectionPointPosition>), typeof(TrackSection));

		/// <summary>
		/// For which signal this track circuit blinks when selected as a pending entrance.
		/// </summary>
		public short? BlinkForSignal {
			get {
				return (short?) GetValue(BlinkForSignalProperty);
			}
			set {
				SetValue(BlinkForSignalProperty, value);
			}
		}

		/// <summary>
		/// The blink for signal property.
		/// </summary>
		public static readonly DependencyProperty BlinkForSignalProperty = DependencyProperty.Register(nameof(BlinkForSignal), typeof(short?), typeof(TrackSection));

		/// <summary>
		/// The list of points and the positions they must be in in order for this section to be part of the current route, in resolved form.
		/// </summary>
		private SectionPointPositionResolved[] PointPositionsResolved;

		/// <summary>
		/// For which signal this track circuit blinks when selected as a pending entrance.
		/// </summary>
		private World.Signal BlinkForSignalResolved;

		/// <summary>
		/// Constructs a new TrackSection.
		/// </summary>
		protected TrackSection() {
			PointPositions = new List<SectionPointPosition>();
			Loaded += OnLoaded;
			RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
		}

		/// <summary>
		/// Updates the graphical appearance of this object.
		/// </summary>
		protected abstract void Update();

		/// <summary>
		/// The colour in which this section should be rendered, based on the state of the track circuit.
		/// </summary>
		protected Brush RenderColour {
			get {
				MainViewModel vm = DataContext as MainViewModel;
				if ((vm != null) && (BlinkForSignalResolved != null) && (vm.PendingEntrance == BlinkForSignalResolved)) {
					return (Brush) FindResource(vm.BlinkClockSource.Value ? "LockedWhite" : "BG");
				} else if (TrackCircuit == null) {
					return (Brush) FindResource(IsKeyedPoint ? "KeyedBlue" : "IdleGrey");
				} else if (TrackCircuit.Occupied) {
					return (Brush) FindResource(ExcludedFromRoute ? "IdleGrey" : "OccupiedRed");
				} else if (IsKeyedPoint) {
					return (Brush) FindResource("KeyedBlue");
				} else if (TrackCircuit.RouteLocked) {
					return (Brush) FindResource(ExcludedFromRoute ? "IdleGrey" : "LockedWhite");
				} else {
					return (Brush) FindResource("IdleGrey");
				}
			}
		}

		/// <summary>
		/// Whether this track section is points that have been keyed.
		/// </summary>
		protected virtual bool IsKeyedPoint {
			get {
				return false;
			}
		}

		/// <summary>
		/// The track circuit that this section is part of.
		/// </summary>
		private TrackCircuit TrackCircuit;

		/// <summary>
		/// Computes whether this section is excluded from the current route due to point positions.
		/// </summary>
		/// <remarks>
		/// This property is <c>false</c> if no route is set, because then the section cannot be excluded from anything.
		/// </remarks>
		private bool ExcludedFromRoute {
			get {
				if ((TrackCircuit != null) && TrackCircuit.RouteLocked) {
					SubArea sub = ((MainViewModel) DataContext).World.Region.SubAreas[SubAreaID];
					foreach (SectionPointPositionResolved i in PointPositionsResolved) {
						if (i.Points.Reversed != i.Reversed) {
							return true;
						}
					}
					return false;
				} else {
					return false;
				}
			}
		}

		/// <summary>
		/// Finishes initialization of this object.
		/// </summary>
		/// <param name="sender">This object.</param>
		/// <param name="e">Details of the event.</param>
		private void OnLoaded(object sender, EventArgs e) {
			MainViewModel vm = DataContext as MainViewModel;
			if (vm != null) {
				if (TrackCircuitID >= 0) {
					TrackCircuit = vm.World.Region.SubAreas[SubAreaID].TrackCircuits[TrackCircuitID];
					TrackCircuit.PropertyChanged += OnTCPropChanged;
				}

				PointPositionsResolved = new SectionPointPositionResolved[PointPositions.Count];
				for (int i = 0; i != PointPositions.Count; ++i) {
					SectionPointPosition p = PointPositions[i];
					PointPositionsResolved[i].Points = vm.World.Region.SubAreas[SubAreaID].PowerPoints[p.Points];
					PointPositionsResolved[i].Reversed = p.Reversed;
					PointPositionsResolved[i].Points.PropertyChanged += OnConditionalPointsPropChanged;
				}
				PointPositions = null;

				if (BlinkForSignal.HasValue) {
					BlinkForSignalResolved = vm.World.Region.SubAreas[SubAreaID].Signals[BlinkForSignal.Value];
					vm.PropertyChanged += OnVMPropChanged;
				}
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

		/// <summary>
		/// Invoked when a property changes on one of the points that determines whether this section is part of the current route.
		/// </summary>
		/// <param name="sender">The points.</param>
		/// <param name="e">Information about the change.</param>
		private void OnConditionalPointsPropChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(World.Points.Reversed)) {
				Update();
			}
		}

		/// <summary>
		/// Invoked when a property changes on the MainViewModel.
		/// </summary>
		/// <param name="sender">The MainViewModel.</param>
		/// <param name="e">Information about the change.</param>
		private void OnVMPropChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(MainViewModel.PendingEntrance)) {
				MainViewModel vm = (MainViewModel) sender;
				vm.BlinkClockSource.PropertyChanged -= OnBlinkChanged;
				if ((BlinkForSignalResolved != null) && (vm.PendingEntrance == BlinkForSignalResolved)) {
					vm.BlinkClockSource.PropertyChanged += OnBlinkChanged;
				}
				Update();
			}
		}

		/// <summary>
		/// Invoked when the blink clock state changes.
		/// </summary>
		/// <param name="sender">The blink clock.</param>
		/// <param name="e">Information about the change.</param>
		private void OnBlinkChanged(object sender, PropertyChangedEventArgs e) {
			Update();
		}
	}
}
