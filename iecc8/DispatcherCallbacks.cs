using System;
using System.Collections.Generic;

namespace Iecc8 {
	class DispatcherCallbacks : IDispatcher {
		private Dictionary<int, List<int>> lastOccupied = new Dictionary<int, List<int>>();
		private Dictionary<int, List<int>> lastManual = new Dictionary<int, List<int>>();
		private Dictionary<int, List<int>> lastReversedSwitches = new Dictionary<int, List<int>>();
		private Dictionary<int, List<Messages.ESignalIndication>> lastSignals = new Dictionary<int, List<Messages.ESignalIndication>>();

		public void DTMF(Messages.DTMFMessage msg) {
		}

		public void PermissionUpdate(Messages.DispatcherPermissionMessage msg) {
		}

		public void Ping() {
		}

		public void RadioText(Messages.RadioTextMessage msg) {
		}

		public void SendSimulationState(Messages.SimulationStateMessage msg) {
		}

		public void SetInterlockErrorSwitches(Messages.InterlockErrorSwitchesMessage msg) {
		}

		public void SetOccupiedBlocks(Messages.OccupiedBlocksMessage msg) {
			UpdateListOfIntegers(lastOccupied, msg.Route, msg.OccupiedBlocks, "block", "occupied", "cleared");
			UpdateListOfIntegers(lastManual, msg.Route, msg.OpenManualSwitchBlocks, "block hand points", "reversed", "normalized");
		}

		public void SetOccupiedSwitches(Messages.OccupiedSwitchesMessage msg) {
		}

		public void SetReversedSwitches(Messages.ReversedSwitchesMessage msg) {
			UpdateListOfIntegers(lastReversedSwitches, msg.Route, msg.ReversedSwitches, "switch", "reversed", "normalized");
		}

		public void SetSignals(Messages.SignalsMessage msg) {
			if (lastSignals.ContainsKey(msg.Route)) {
				List<Messages.ESignalIndication> old = lastSignals[msg.Route];
				int max = Math.Max(old.Count, msg.Signals.Count);
				for (int i = 0; i != max; ++i) {
					if (i >= old.Count) {
						Console.WriteLine("Route " + msg.Route + " signal number " + i + " added when it used not to exist.");
					} else if (i >= msg.Signals.Count) {
						Console.WriteLine("Route " + msg.Route + " signal number " + i + " disappeared.");
					} else if (msg.Signals[i] != old[i]) {
						Console.WriteLine("Route " + msg.Route + " signal number " + i + " changed from " + old[i] + " to " + msg.Signals[i] + ".");
					}
				}
				lastSignals[msg.Route] = msg.Signals;
			} else {
				Console.WriteLine("Initializing signals for route " + msg.Route);
				lastSignals[msg.Route] = msg.Signals;
			}
		}

		public void SetUnlockedSwitches(Messages.UnlockedSwitchesMessage msg) {
		}

		public void UpdateTrainData(Messages.TrainDataMessage msg) {
		}

		private static void UpdateListOfIntegers(Dictionary<int, List<int>> lastDict, int route, List<int> items, string objectName, string addedText, string removedText) {
			items.Sort();
			if (lastDict.ContainsKey(route)) {
				List<int> last = lastDict[route];
				int i = 0, j = 0;
				while ((i < last.Count) || (j < items.Count)) {
					if ((i < last.Count) && (j < items.Count) && (last[i] == items[j])) {
						// Equal
						++i;
						++j;
					} else if ((i < last.Count) && ((j == items.Count) || (last[i] < items[j]))) {
						// element i is less than element j
						Console.WriteLine("Route " + route + " " + objectName + " number " + last[i] + " " + removedText + ".");
						++i;
					} else {
						// element j is less than element i
						Console.WriteLine("Route " + route + " " + objectName + " number " + items[j] + " " + addedText + ".");
						++j;
					}
				}
				lastDict[route] = items;
			} else {
				Console.WriteLine("Initializing " + objectName + " table for route " + route);
				lastDict[route] = items;
			}
		}
	}
}
