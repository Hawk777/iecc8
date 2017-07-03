using Iecc8.UI.Common;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Iecc8.UI.Equipment {
	/// <summary>
	/// Interaction logic for AutoButton.xaml
	/// </summary>
	public partial class AutoButton : Grid {
		/// <summary>
		/// Which sub-area this signal is in.
		/// </summary>
		[Category("Track"), Description("Gets or sets the ID number of the subdivision containing the signal.")]
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
		public static readonly DependencyProperty SubAreaIDProperty = DependencyProperty.Register(nameof(SubAreaID), typeof(ushort), typeof(AutoButton));

		/// <summary>
		/// The ID number of the signal.
		/// </summary>
		[Category("Track"), Description("Gets or sets the ID number of the signal.")]
		public short SignalID {
			get {
				return (short) GetValue(SignalIDProperty);
			}
			set {
				SetValue(SignalIDProperty, value);
			}
		}

		/// <summary>
		/// The signal ID property.
		/// </summary>
		public static readonly DependencyProperty SignalIDProperty = DependencyProperty.Register(nameof(SignalID), typeof(short), typeof(AutoButton));

		/// <summary>
		/// The signal.
		/// </summary>
		private World.ControlledSignal SignalObject;

		/// <summary>
		/// Constructs a new AutoButton.
		/// </summary>
		public AutoButton() {
			InitializeComponent();
			Loaded += OnLoaded;
		}

		/// <summary>
		/// Finishes initialization of this object.
		/// </summary>
		/// <param name="sender">This object.</param>
		/// <param name="e">Details of the event.</param>
		private void OnLoaded(object sender, EventArgs e) {
			MainViewModel vm = DataContext as MainViewModel;
			if (vm != null) {
				SignalObject = vm.World.Region.SubAreas[SubAreaID].ControlledSignals[SignalID];
				SignalObject.PropertyChanged += OnSignalPropChanged;
			}
			Update();
		}

		/// <summary>
		/// Fired when a property of the signal has changed.
		/// </summary>
		/// <param name="sender">The signal.</param>
		/// <param name="e">Information about the event.</param>
		private void OnSignalPropChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(World.ControlledSignal.AutoWorking)) {
				Update();
			}
		}

		/// <summary>
		/// Updates the UI elements based on the current situation.
		/// </summary>
		private void Update() {
			if ((SignalObject == null) || SignalObject.AutoWorking) {
				Roundel.Fill = (Brush) FindResource("SignalAutoButton");
				Roundel.Stroke = null;
			} else {
				Roundel.Fill = null;
				Roundel.Stroke = (Brush) FindResource("SignalAutoButton");
			}
		}

		/// <summary>
		/// Handles a mouse button being pressed.
		/// </summary>
		/// <param name="e">Details of the event.</param>
		protected override async void OnMouseDown(MouseButtonEventArgs e) {
			base.OnMouseDown(e);
			if (SignalObject != null) {
				if (e.ChangedButton == MouseButton.Left) {
					if (SignalObject.AutoWorkingAvailable) {
						await SignalObject.SetAutoWorkingAsync(true);
					} else {
						MainViewModel vm = DataContext as MainViewModel;
						if (vm != null) {
							Message msg = default(Message);
							msg.Text = "Auto working not available";
							msg.Type = Message.EType.Miscellaneous;
							vm.Messages.Add(msg);
						}
					}
				} else if (e.ChangedButton == MouseButton.Right) {
					await SignalObject.SetAutoWorkingAsync(false);
				}
			}
		}
	}
}
