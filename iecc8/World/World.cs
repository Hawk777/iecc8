using Iecc8.Messages;
using Iecc8.Schema;
using System;
using System.Diagnostics;
using System.Threading;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace Iecc8.World {
	/// <summary>
	/// The outer access point of the model classes.
	/// </summary>
	/// <remarks>
	/// This object dispatches notifications to model classes that need to receive them. It also catches communication errors (to prevent them from bubbling out too far) and translates the status of the communication link into a property.
	/// </remarks>
	public class World : BindableBase, IDispatcher {
		#region Signaller API
		/// <summary>
		/// Whether the communication link to Run8 has failed.
		/// </summary>
		public bool LinkError {
			get {
				return LinkErrorImpl;
			}
			private set {
				SetProperty(ref LinkErrorImpl, value);
			}
		}

		/// <summary>
		/// The permission level granted to the dispatcher.
		/// </summary>
		public EDispatcherPermission Permission {
			get {
				return PermissionImpl;
			}
			private set {
				SetProperty(ref PermissionImpl, value);
			}
		}

		/// <summary>
		/// Whether the dispatcher is allowed to send orders to AI drivers.
		/// </summary>
		public bool AIPermission {
			get {
				return AIPermissionImpl;
			}
			private set {
				SetProperty(ref AIPermissionImpl, value);
			}
		}

		/// <summary>
		/// The current in-game date and time.
		/// </summary>
		public DateTime SimulationTime {
			get {
				return SimulationTimeImpl;
			}
			private set {
				SetProperty(ref SimulationTimeImpl, value);
			}
		}

		/// <summary>
		/// The region being communicated with.
		/// </summary>
		public readonly Region Region;
		#endregion

		#region Run8 API
		// All functions in this region are called on the thread pool by WCF, so any touching of the Route objects, emitting of events, etc. must be posted to SyncContext.

		void IDispatcher.DTMF(DTMFMessage pMessage) {
			MessageReceived = true;
		}

		void IDispatcher.PermissionUpdate(DispatcherPermissionMessage pMessage) {
			MessageReceived = true;
			SyncContext.Post((object state) => {
				Permission = pMessage.Permission;
				AIPermission = pMessage.AIPermission;
			}, null);
		}

		void IDispatcher.Ping() {
			MessageReceived = true;
		}

		void IDispatcher.RadioText(RadioTextMessage pMessage) {
			MessageReceived = true;
		}

		void IDispatcher.SendSimulationState(SimulationStateMessage pMessage) {
			MessageReceived = true;
			SyncContext.Post((object state) => SimulationTime = pMessage.SimulationTime, null);
		}

		void IDispatcher.SetInterlockErrorSwitches(InterlockErrorSwitchesMessage pMessage) {
			MessageReceived = true;
			SyncContext.Post((object state) => Region.UpdateFromRun8(pMessage), null);
		}

		void IDispatcher.SetOccupiedBlocks(OccupiedBlocksMessage pMessage) {
			MessageReceived = true;
			SyncContext.Post((object state) => Region.UpdateFromRun8(pMessage), null);
		}

		void IDispatcher.SetOccupiedSwitches(OccupiedSwitchesMessage pMessage) {
			MessageReceived = true;
			SyncContext.Post((object state) => Region.UpdateFromRun8(pMessage), null);
		}

		void IDispatcher.SetReversedSwitches(ReversedSwitchesMessage pMessage) {
			MessageReceived = true;
			SyncContext.Post((object state) => Region.UpdateFromRun8(pMessage), null);
		}

		void IDispatcher.SetSignals(SignalsMessage pMessage) {
			MessageReceived = true;
			SyncContext.Post(async (object state) => {
				try {
					await Region.UpdateFromRun8Async(pMessage);
				} catch (CommunicationException) {
					LinkError = true;
				}
			}, null);
		}

		void IDispatcher.SetUnlockedSwitches(UnlockedSwitchesMessage pMessage) {
			MessageReceived = true;
			SyncContext.Post((object state) => Region.UpdateFromRun8(pMessage), null);
		}

		void IDispatcher.UpdateTrainData(TrainDataMessage pMessage) {
			MessageReceived = true;
		}
		#endregion

		#region Data Initialization API
		/// <summary>
		/// Thrown if an attempt is made to construct a world with a region ID that is not recognized.
		/// </summary>
		public class UnrecognizedRegionException : ApplicationException {
		}

		/// <summary>
		/// Constructs a new World.
		/// </summary>
		/// <param name="regionID">The ID number of the region to create.</param>
		public World(Run8Wrapper run8, int regionID) {
			Debug.Assert(run8 != null);
			PermissionImpl = EDispatcherPermission.Rescinded;
			AIPermissionImpl = false;
			Run8 = run8;
			SyncContext = SynchronizationContext.Current;
			PingTimer = new Timer(PingTimerTick, null, 0, 5000);
			Regions regions = (Regions) Application.LoadComponent(new Uri("/iecc8;component/Region/Regions.xaml", UriKind.Relative));
			Schema.Region region = null;
			foreach (Schema.Region i in regions) {
				foreach (RegionEntry j in i.SubAreas) {
					if (j.ID == regionID) {
						region = i;
						break;
					}
				}
			}
			if (region == null) {
				throw new UnrecognizedRegionException();
			}
			Region = new Region(region, this);
		}
		#endregion

		#region Interlocking API
		/// <summary>
		/// Chooses whether a signal is on or off and, if off, whether it has automatic working enabled.
		/// </summary>
		/// <param name="route">The route containing the signal.</param>
		/// <param name="signal">The ID number of the signal.</param>
		/// <param name="indication">Which indication to set.</param>
		public async Task ChangeSignalAsync(int route, int signal, ESignalIndication indication) {
			try {
				await Run8.ChangeSignalAsync(route, signal, indication);
			} catch (CommunicationException) {
				LinkError = true;
			}
		}

		/// <summary>
		/// Drives power points to a position, unlocks them for hand cranking, or relocks them for power operation.
		/// </summary>
		/// <param name="route">Which sub-area the points are in.</param>
		/// <param name="points">Which points to modify.</param>
		/// <param name="state">What action the points should take.</param>
		public async Task ThrowSwitchAsync(int route, int points, ESwitchState state) {
			try {
				await Run8.ThrowSwitchAsync(route, points, state);
			} catch (CommunicationException) {
				LinkError = true;
			}
		}
		#endregion

		#region Private Members
		private bool LinkErrorImpl;
		private EDispatcherPermission PermissionImpl;
		private bool AIPermissionImpl;
		private DateTime SimulationTimeImpl;
		private volatile bool MessageReceived;
		private readonly Run8Wrapper Run8;
		private readonly SynchronizationContext SyncContext;
		private readonly Timer PingTimer;

		/// <summary>
		/// Pings Run8 if no message has been received for a while.
		/// </summary>
		private void PingTimerTick(object state) {
			bool shouldPing = !MessageReceived;
			MessageReceived = false;
			if (shouldPing) {
				try {
					Run8.PingAsync().Wait();
				} catch (CommunicationException) {
					PingTimer.Change(Timeout.Infinite, Timeout.Infinite);
					SyncContext.Post((object param) => LinkError = true, null);
				}
			}
		}
		#endregion
	}
}
