using Iecc8.World;
using System.Diagnostics;

namespace Iecc8.UI {
	public class MainViewModel : BindableBase {
		/// <summary>
		/// The world being interacted with.
		/// </summary>
		public World.World World {
			get {
				return WorldImpl;
			}
		}

		/// <summary>
		/// The clock source to be used by blinking UI elements.
		/// </summary>
		public BlinkClockSource BlinkClockSource {
			get {
				return BlinkClockSourceImpl;
			}
		}

		/// <summary>
		/// The signal that has been selected as the entrance for the next route to call.
		/// </summary>
		public ControlledSignal PendingEntrance {
			get {
				return PendingEntranceImpl;
			}
			set {
				SetProperty(ref PendingEntranceImpl, value);
			}
		}

		/// <summary>
		/// Constructs a new MainViewModel.
		/// </summary>
		/// <param name="world">The world being interacted with.</param>
		public MainViewModel(World.World world) {
			Debug.Assert(world != null);
			WorldImpl = world;
			BlinkClockSourceImpl = new BlinkClockSource();
		}

		/// <summary>
		/// Handles the signaller selecting a signal.
		/// </summary>
		/// <param name="signal">The selected signal.</param>
		public async void SelectSignal(World.Signal signal) {
			if (PendingEntrance != null) {
				ControlledSignal entrance = PendingEntrance;
				PendingEntrance = null;
				Route route;
				if (entrance.RoutesFrom.TryGetValue(signal, out route)) {
					if (route.Available) {
						await route.CallAsync();
					}
				}
			} else {
				ControlledSignal cs = signal as ControlledSignal;
				if (cs != null) {
					PendingEntrance = cs;
				}
			}
		}

		/// <summary>
		/// Handles the signaller cancelling a signal.
		/// </summary>
		/// <param name="signal">The signal.</param>
		public void CancelSignal(World.Signal signal) {
			if (PendingEntrance == signal) {
				PendingEntrance = null;
			} else {
				ControlledSignal cs = signal as ControlledSignal;
				if (cs != null) {
					cs.Cancel();
				}
			}
		}

		/// <summary>
		/// Handles the signaller asking to flag by a signal or cancel flag-by mode.
		/// </summary>
		/// <param name="signal">The signal.</param>
		public void FlagBySignal(World.Signal signal) {
			ControlledSignal cs = signal as ControlledSignal;
			if (cs != null) {
				if (cs.CurrentRoute == null) {
					cs.EnableFlagBy();
				}
			}
		}

		private readonly World.World WorldImpl;
		private readonly BlinkClockSource BlinkClockSourceImpl;
		private ControlledSignal PendingEntranceImpl;
	}
}
