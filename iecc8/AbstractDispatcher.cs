using Iecc8.Messages;

namespace Iecc8 {
	/// <summary>
	/// An implementation of IDispatcher that doesn't do anything.
	/// </summary>
	public abstract class AbstractDispatcher : IDispatcher {
		public virtual void DTMF(DTMFMessage pMessage) {
		}

		public virtual void PermissionUpdate(DispatcherPermissionMessage pMessage) {
		}

		public virtual void Ping() {
		}

		public virtual void RadioText(RadioTextMessage pMessage) {
		}

		public virtual void SendSimulationState(SimulationStateMessage pMessage) {
		}

		public virtual void SetInterlockErrorSwitches(InterlockErrorSwitchesMessage pMessage) {
		}

		public virtual void SetOccupiedBlocks(OccupiedBlocksMessage pMessage) {
		}

		public virtual void SetOccupiedSwitches(OccupiedSwitchesMessage pMessage) {
		}

		public virtual void SetReversedSwitches(ReversedSwitchesMessage pMessage) {
		}

		public virtual void SetSignals(SignalsMessage pMessage) {
		}

		public virtual void SetUnlockedSwitches(UnlockedSwitchesMessage pMessage) {
		}

		public virtual void UpdateTrainData(TrainDataMessage pMessage) {
		}
	}
}
