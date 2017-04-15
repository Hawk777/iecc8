using Iecc8.Messages;
using Iecc8.World;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Iecc8.UI {
	/// <summary>
	/// Interaction logic for TDBerth.xaml
	/// </summary>
	public partial class TDBerth : TextBlock {
		/// <summary>
		/// Which sub-area this berth is in.
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
		public static readonly DependencyProperty SubAreaIDProperty = DependencyProperty.Register(nameof(SubAreaID), typeof(ushort), typeof(TDBerth));

		/// <summary>
		/// The track circuit that this bert is associated with.
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
		public static readonly DependencyProperty TrackCircuitIDProperty = DependencyProperty.Register(nameof(TrackCircuitID), typeof(int), typeof(TDBerth));

		/// <summary>
		/// The train currently showing in this berth.
		/// </summary>
		public Train Train {
			get {
				return (Train) GetValue(TrainProperty);
			}
			set {
				SetValue(TrainProperty, value);
			}
		}

		/// <summary>
		/// The train property.
		/// </summary>
		public static readonly DependencyProperty TrainProperty = DependencyProperty.Register(nameof(Train), typeof(Train), typeof(TDBerth));

		/// <summary>
		/// Constructs a new TDBerth.
		/// </summary>
		public TDBerth() {
			InitializeComponent();
			Loaded += OnLoaded;
		}

		protected override void OnPreviewMouseDown(MouseButtonEventArgs e) {
			if (e.ChangedButton == MouseButton.Left) {
				MainViewModel vm = DataContext as MainViewModel;
				if (vm != null) {
					vm.TrainDetailViewModel.SetTrains(new List<Train>(vm.World.Region.SubAreas[SubAreaID].TrackCircuits[TrackCircuitID].Trains));
				}
			}
		}

		private TrackCircuit TCObject;

		private void OnLoaded(object sender, EventArgs e) {
			MainViewModel vm = DataContext as MainViewModel;
			if (vm != null) {
				TCObject = vm.World.Region.SubAreas[SubAreaID].TrackCircuits[TrackCircuitID];
				TCObject.Trains.CollectionChanged += OnTrainsChanged;
			}
			UpdateTrain();
		}

		private void OnTrainsChanged(object sender, NotifyCollectionChangedEventArgs e) {
			UpdateTrain();
		}

		private void UpdateTrain() {
			if (TCObject != null) {
				Train = (TCObject.Trains.Count != 0) ? TCObject.Trains[0] : null;
			} else {
				TrainData data = default(TrainData);
				data.BlockID = -1;
				data.LocoNumber = TrackCircuitID;
				Train = new Train(data, null);
			}
		}
	}
}
