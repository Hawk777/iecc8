using Iecc8.World;
using System;
using System.Collections.ObjectModel;
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
		/// The messages.
		/// </summary>
		public ObservableCollection<Message> Messages {
			get;
			private set;
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
			Messages = new ObservableCollection<Message>();
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
					} else {
						AddMessage(Message.EType.Miscellaneous, "Route not available", false);
					}
				} else {
					AddMessage(Message.EType.Miscellaneous, "No route between selected signals", false);
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

		/// <summary>
		/// Clears the message at a specified position in the message list.
		/// </summary>
		/// <remarks>
		/// This does nothing if the message list has been added to within the last second. This avoids race conditions where the user accidentally deletes the wrong message because the list scrolls right when they are about to click.
		/// </remarks>
		/// <param name="index">The position.</param>
		public void ClearMessage(int index) {
			if (DateTime.UtcNow > InhibitDeletingMessageUntil) {
				if ((0 <= index) && (index < Messages.Count)) {
					Messages.RemoveAt(index);
				}
			}
		}

		private readonly World.World WorldImpl;
		private readonly BlinkClockSource BlinkClockSourceImpl;
		private ControlledSignal PendingEntranceImpl;
		private DateTime InhibitDeletingMessageUntil;

		/// <summary>
		/// Adds a new message to the messages list.
		/// </summary>
		/// <param name="type">The type of message to add.</param>
		/// <param name="text">The text of the message to add.</param>
		/// <param name="spontaneous"><c>true</c> if this message occurred due to something happening in the world, or <c>false</c> if it was in direct response to the signaller's actions.</param>
		private void AddMessage(Message.EType type, string text, bool spontaneous) {
			Message m = new Message();
			m.Type = type;
			m.Text = text;
			Messages.Add(m);
			if (spontaneous) {
				InhibitDeletingMessageUntil = DateTime.UtcNow.AddSeconds(1);
			}
		}
	}
}
