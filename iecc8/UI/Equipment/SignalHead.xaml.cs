using Iecc8.UI.Common;
using Iecc8.World;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace Iecc8.UI.Equipment {
	/// <summary>
	/// Interaction logic for SignalHead.xaml
	/// </summary>
	public partial class SignalHead : UserControl {
		/// <summary>
		/// Constructs a new SignalHead.
		/// </summary>
		/// <param name="signal">Which signal this head is part of.</param>
		/// <param name="head">Which position this head is in, counting from the top.</param>
		/// <param name="blinkClockSource">The source of the blinking clock.</param>
		public SignalHead(World.Signal signal, byte head, BlinkClockSource blinkClockSource) {
			InitializeComponent();
			SignalObject = signal;
			if (SignalObject != null) {
				SignalObject.PropertyChanged += OnSignalPropChanged;
			}
			Head = head;
			BlinkClockSource = blinkClockSource;
			UpdateColour();
		}

		private readonly World.Signal SignalObject;
		private readonly byte Head;
		private readonly BlinkClockSource BlinkClockSource;

		/// <summary>
		/// Invoked when a property changes on the observed signal.
		/// </summary>
		/// <param name="sender">The signal.</param>
		/// <param name="e">Details about the change.</param>
		private void OnSignalPropChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(World.Signal.Aspects) || e.PropertyName == nameof(ControlledSignal.ApproachLocked)) {
				UpdateColour();
			}
		}

		/// <summary>
		/// Invoked when the blink clock ticks.
		/// </summary>
		/// <param name="sender">The blink clock.</param>
		/// <param name="e">Details about the change.</param>
		private void OnBlinkClockChanged(object sender, PropertyChangedEventArgs e) {
			UpdateColour();
		}

		/// <summary>
		/// Updates the fill colour of the lamp based on the signal's aspect.
		/// </summary>
		private void UpdateColour() {
			EAspect aspect = (SignalObject != null) ? SignalObject.Aspects.AspectAt(Head) : EAspect.Red;
			ControlledSignal cs = SignalObject as ControlledSignal;
			bool approachLocked = (cs != null) ? cs.ApproachLocked : false;
			switch (aspect) {
				case EAspect.Red:
					if ((BlinkClockSource == null) || BlinkClockSource.Value || !approachLocked) {
						Lamp.Fill = (Brush) FindResource("SignalRed");
					} else {
						Lamp.Fill = (Brush) FindResource("BG");
					}
					break;
				case EAspect.Green:
					Lamp.Fill = (Brush) FindResource("SignalGreen");
					break;
				case EAspect.Yellow:
					Lamp.Fill = (Brush) FindResource("SignalYellow");
					break;
				case EAspect.FlashingYellow:
					if ((BlinkClockSource == null) || BlinkClockSource.Value) {
						Lamp.Fill = (Brush) FindResource("SignalYellow");
					} else {
						Lamp.Fill = (Brush) FindResource("BG");
					}
					break;
				case EAspect.Lunar:
					Lamp.Fill = (Brush) FindResource("SignalLunar");
					break;
			}
			if (BlinkClockSource != null) {
				BlinkClockSource.PropertyChanged -= OnBlinkClockChanged;
				if ((aspect == EAspect.FlashingYellow) || approachLocked) {
					BlinkClockSource.PropertyChanged += OnBlinkClockChanged;
				}
			}
		}
	}
}
