using System.Threading;

namespace Iecc8.UI.Common {
	/// <summary>
	/// A source of a half-Hertz clock signal for blinking UI elements.
	/// </summary>
	public class BlinkClockSource : BindableBase {
		/// <summary>
		/// The value of the clock source.
		/// </summary>
		public bool Value {
			get {
				return ValueImpl;
			}
			private set {
				SetProperty(ref ValueImpl, value);
			}
		}

		/// <summary>
		/// Constructs a new BlinkClockSource.
		/// </summary>
		public BlinkClockSource() {
			SyncContext = SynchronizationContext.Current;
			Timer = new Timer(Tick, null, 500, 500);
		}

		private bool ValueImpl;
		private readonly SynchronizationContext SyncContext;
		private readonly Timer Timer;

		/// <summary>
		/// Handles a timer tick.
		/// </summary>
		private void Tick(object param) {
			SyncContext.Post((object param2) => Value = !Value, null);
		}
	}
}
