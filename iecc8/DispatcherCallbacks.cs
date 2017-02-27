using System;

namespace Iecc8 {
	class DispatcherCallbacks : IDispatcher {
		public void DTMF(Messages.DTMFMessage msg) {
			Console.WriteLine("< DTMF(" + msg + ")");
		}

		public void PermissionUpdate(Messages.DispatcherPermissionMessage msg) {
			Console.WriteLine("< PermissionUpdate(" + msg + ")");
		}

		public void Ping() {
			Console.WriteLine("< Ping()");
		}

		public void RadioText(Messages.RadioTextMessage msg) {
			Console.WriteLine("< RadioTextMessage(" + msg + ")");
		}

		public void SendSimulationState(Messages.SimulationStateMessage msg) {
			Console.WriteLine("< SendSimulationState(" + msg + ")");
		}

		public void SetInterlockErrorSwitches(Messages.InterlockErrorSwitchesMessage msg) {
			Console.WriteLine("< SetInterlockErrorSwitches(" + msg + ")");
		}

		public void SetOccupiedBlocks(Messages.OccupiedBlocksMessage msg) {
			Console.WriteLine("< SetOccupiedBlocks(" + msg + ")");
		}

		public void SetOccupiedSwitches(Messages.OccupiedSwitchesMessage msg) {
			Console.WriteLine("< SetOccupiedSwitches(" + msg + ")");
		}

		public void SetReversedSwitches(Messages.ReversedSwitchesMessage msg) {
			Console.WriteLine("< SetReversedSwitches(" + msg + ")");
		}

		public void SetSignals(Messages.SignalsMessage msg) {
			Console.WriteLine("< SetSignals(" + msg + ")");
		}

		public void SetUnlockedSwitches(Messages.UnlockedSwitchesMessage msg) {
			Console.WriteLine("< SetUnlockedSwitches(" + msg + ")");
		}

		public void UpdateTrainData(Messages.TrainDataMessage msg) {
			Console.WriteLine("< UpdateTrainData(" + msg + ")");
		}
	}
}
