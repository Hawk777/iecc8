using Iecc8.Messages;

namespace Iecc8 {
	/// <summary>
	/// An implementation of IDispatcher that delivers all received messages to another IDispatcher.
	/// </summary>
	/// <remarks>
	/// This allows the recipient of dispatcher messages to be changed after the communication channel is open, which would not normally be possible as Windows Communication Foundation does not permit changing a channel's InstanceContext after opening. It also allows messages to be temporarily ignored if desired.
	/// </remarks>
	public class DispatcherProxy : IDispatcher {
		/// <summary>
		/// Changes where messages are sent.
		/// </summary>
		/// <param name="target">The target object to pass the received messages to, or <c>null</c> to discard the messages.</param>
		public void SetTarget(IDispatcher target) {
			lock (this) {
				Target = target;
			}
		}

		void IDispatcher.DTMF(DTMFMessage pMessage) {
			IDispatcher target;
			lock (this) {
				target = Target;
			}
			if (target != null) {
				target.DTMF(pMessage);
			}
		}

		void IDispatcher.PermissionUpdate(DispatcherPermissionMessage pMessage) {
			IDispatcher target;
			lock (this) {
				target = Target;
			}
			if (target != null) {
				target.PermissionUpdate(pMessage);
			}
		}

		void IDispatcher.Ping() {
			IDispatcher target;
			lock (this) {
				target = Target;
			}
			if (target != null) {
				target.Ping();
			}
		}

		void IDispatcher.RadioText(RadioTextMessage pMessage) {
			IDispatcher target;
			lock (this) {
				target = Target;
			}
			if (target != null) {
				target.RadioText(pMessage);
			}
		}

		void IDispatcher.SendSimulationState(SimulationStateMessage pMessage) {
			IDispatcher target;
			lock (this) {
				target = Target;
			}
			if (target != null) {
				target.SendSimulationState(pMessage);
			}
		}

		void IDispatcher.SetInterlockErrorSwitches(InterlockErrorSwitchesMessage pMessage) {
			IDispatcher target;
			lock (this) {
				target = Target;
			}
			if (target != null) {
				target.SetInterlockErrorSwitches(pMessage);
			}
		}

		void IDispatcher.SetOccupiedBlocks(OccupiedBlocksMessage pMessage) {
			IDispatcher target;
			lock (this) {
				target = Target;
			}
			if (target != null) {
				target.SetOccupiedBlocks(pMessage);
			}
		}

		void IDispatcher.SetOccupiedSwitches(OccupiedSwitchesMessage pMessage) {
			IDispatcher target;
			lock (this) {
				target = Target;
			}
			if (target != null) {
				target.SetOccupiedSwitches(pMessage);
			}
		}

		void IDispatcher.SetReversedSwitches(ReversedSwitchesMessage pMessage) {
			IDispatcher target;
			lock (this) {
				target = Target;
			}
			if (target != null) {
				target.SetReversedSwitches(pMessage);
			}
		}

		void IDispatcher.SetSignals(SignalsMessage pMessage) {
			IDispatcher target;
			lock (this) {
				target = Target;
			}
			if (target != null) {
				target.SetSignals(pMessage);
			}
		}

		void IDispatcher.SetUnlockedSwitches(UnlockedSwitchesMessage pMessage) {
			IDispatcher target;
			lock (this) {
				target = Target;
			}
			if (target != null) {
				target.SetUnlockedSwitches(pMessage);
			}
		}

		void IDispatcher.UpdateTrainData(TrainDataMessage pMessage) {
			IDispatcher target;
			lock (this) {
				target = Target;
			}
			if (target != null) {
				target.UpdateTrainData(pMessage);
			}
		}

		/// <summary>
		/// Which object to send the incoming messages to.
		/// </summary>
		private IDispatcher Target;
	}
}
