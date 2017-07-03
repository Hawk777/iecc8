using Iecc8.UI.Common;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Iecc8.UI.Equipment {
	/// <summary>
	/// The base class for signal and signal-like UI elements.
	/// </summary>
	public abstract class SignalBase : UserControl {
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
		public static readonly DependencyProperty SubAreaIDProperty = DependencyProperty.Register(nameof(SubAreaID), typeof(ushort), typeof(SignalBase));

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
		public static readonly DependencyProperty SignalIDProperty = DependencyProperty.Register(nameof(SignalID), typeof(short), typeof(SignalBase));

		/// <summary>
		/// The signal.
		/// </summary>
		public World.Signal SignalObject {
			get; private set;
		}

		/// <summary>
		/// Constructs a new SignalBase.
		/// </summary>
		public SignalBase() {
			Loaded += OnLoaded;
		}

		/// <summary>
		/// Initializes the signal's UI.
		/// </summary>
		/// <remarks>
		/// SignalObject is set by the time this function is called.
		/// </remarks>
		protected abstract void InitUI();

		/// <summary>
		/// Handles a mouse button being pressed.
		/// </summary>
		/// <param name="e">Details of the event.</param>
		protected override void OnMouseDown(MouseButtonEventArgs e) {
			base.OnMouseDown(e);
			MainViewModel vm = DataContext as MainViewModel;
			if (vm != null) {
				if (e.ChangedButton == MouseButton.Left) {
					vm.SelectSignal(SignalObject);
				} else if (e.ChangedButton == MouseButton.Right) {
					vm.CancelSignal(SignalObject);
				} else if (e.ChangedButton == MouseButton.Middle) {
					vm.FlagBySignal(SignalObject);
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
				SignalObject = vm.World.Region.SubAreas[SubAreaID].Signals[SignalID];
			}
			InitUI();
		}
	}
}
