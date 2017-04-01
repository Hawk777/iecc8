using Iecc8.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Iecc8.World {
	/// <summary>
	/// A set of power-operated points.
	/// </summary>
	public class Points : BindableBase {
		#region Common API
		/// <summary>
		/// Which sub-area contains these points.
		/// </summary>
		public readonly ushort SubArea;

		/// <summary>
		/// The Run8 internal ID number of these points.
		/// </summary>
		public readonly ushort ID;

		/// <summary>
		/// The name of these points.
		/// </summary>
		public string Name {
			get {
				return SubArea + "/" + ID;
			}
		}

		/// <summary>
		/// Whether these points are free to move under interlocking control.
		/// </summary>
		public bool Movable {
			get {
				if (HandCrankable || Keyed || Occupied) {
					return false;
				}
				foreach (TrackCircuit i in ProtectingTCs) {
					if (i.RouteLocked) {
						return false;
					}
				}
				return true;
			}
		}

		/// <summary>
		/// Whether these points are configured for hand cranking.
		/// </summary>
		public bool HandCrankable {
			get {
				return HandCrankableImpl;
			}
			set {
				SetHandCrankable(value);
			}
		}

		/// <summary>
		/// Whether these points are hand-cranked to an inconsistent position.
		/// </summary>
		public bool Inconsistent {
			get {
				return InconsistentImpl;
			}
			private set {
				SetProperty(ref InconsistentImpl, value);
			}
		}

		/// <summary>
		/// The position of these points.
		/// </summary>
		/// <remarks>
		/// If any protecting track circuit is route locked or the points are keyed, this property reflects the requested position of the points, and Proved should be examined to determine whether they are actually in this position. If no protecting track circuit is route locked and the points are unkeyed, this property reflects the actual position of the points, and Proved is always true.
		/// </remarks>
		public bool Reversed {
			get {
				return ReversedImpl;
			}
			private set {
				SetProperty(ref ReversedImpl, value);
			}
		}

		/// <summary>
		/// Whether the position in Reversed is known to be accurate at the lineside.
		/// </summary>
		public bool Proved {
			get {
				return ProvedImpl;
			}
			private set {
				SetProperty(ref ProvedImpl, value);
			}
		}

		/// <summary>
		/// Whether the signaller has keyed the points to their current position.
		/// </summary>
		public bool Keyed {
			get {
				return KeyedImpl;
			}
			private set {
				if (SetProperty(ref KeyedImpl, value)) {
					EmitPropertyChanged(nameof(Movable));
				}
			}
		}

		/// <summary>
		/// Whether there is currently a train over the points.
		/// </summary>
		public bool Occupied {
			get {
				return OccupiedImpl;
			}
			private set {
				if (SetProperty(ref OccupiedImpl, value)) {
					EmitPropertyChanged(nameof(Movable));
				}
			}
		}
		#endregion

		#region Signaller API
		/// <summary>
		/// Keys the points to a requested position.
		/// </summary>
		/// <remarks>
		/// Either Movable must be true, or Reversed must match the requested position.
		/// </remarks>
		/// <param name="reverse">The requested position of the points.</param>
		public Task KeyAsync(bool reverse) {
			Debug.Assert(Movable || (Reversed == reverse));
			Keyed = true;
			return SwingAsync(reverse);
		}

		/// <summary>
		/// Removes the keying from the points.
		/// </summary>
		public void Unkey() {
			Keyed = false;
		}

		/// <summary>
		/// Checks whether the points can be unlocked for hand cranking at this time.
		/// </summary>
		/// <returns><c>true</c> if it is safe to unlock the points, or <c>false</c> if not.</returns>
		public bool CheckHandCrankingAvailable() {
			if (Keyed) {
				return false;
			}
			foreach (TrackCircuit i in ProtectingTCs) {
				if (i.RouteLocked) {
					return false;
				}
			}
			return true;
		}
		#endregion

		#region Run8 API
		public void UpdateInterlockErrorFromRun8(bool interlockError) {
			Inconsistent = interlockError;
		}

		public void UpdateOccupiedFromRun8(bool occupied) {
			Occupied = occupied;
		}

		public void UpdateReversedFromRun8(bool reversed) {
			if (AnyProtectingTCRouteLocked) {
				Proved = Reversed == reversed;
			} else {
				Reversed = reversed;
				Proved = true;
			}
		}

		public void UpdateUnlockedFromRun8(bool unlocked) {
			// Update the impl directly because the property setter is used to actually lock or unlock the points by sending a packet to Run8.
			if (SetProperty(ref HandCrankableImpl, unlocked, nameof(HandCrankable))) {
				EmitPropertyChanged(nameof(Movable));
			}
		}
		#endregion

		#region Data Initialization API
		/// <summary>
		/// Constructs a Points.
		/// </summary>
		/// <param name="schema">The schema object containing data about these points.</param>
		/// <param name="id">The Run8 internal ID number of these points.</param>
		/// <param name="subArea">The sub-area that contains these points.</param>
		/// <param name="world">The containing world.</param>
		public Points(Schema.Points schema, ushort id, SubArea subArea, World world) {
			Debug.Assert(id >= 0);
			Debug.Assert(world != null);
			SubArea = subArea.ID;
			ID = id;
			World = world;
			OccupiedImpl = true;
			ProvedImpl = false;

			{
				TrackCircuit[] protectingTCs = new TrackCircuit[schema.ProtectingTCs.Count];
				for (int i = 0; i != schema.ProtectingTCs.Count; ++i) {
					protectingTCs[i] = subArea.TrackCircuits[schema.ProtectingTCs[i]];
					protectingTCs[i].PropertyChanged += OnTCPropChanged;
				}
				ProtectingTCs = Array.AsReadOnly(protectingTCs);
			}
		}
		#endregion

		#region Interlocking Internal API
		/// <summary>
		/// Starts swinging these points.
		/// </summary>
		/// <param name="reverse">The position to swing to.</param>
		public async Task SwingAsync(bool reverse) {
			if (Reversed != reverse || !Proved) {
				Reversed = reverse;
				Proved = false;
			}
			await World.ThrowSwitchAsync(SubArea, ID, reverse ? ESwitchState.Reversed : ESwitchState.Normal);
		}
		#endregion

		#region Private Members
		/// <summary>
		/// The track circuits that, when occupied or locked, prevent these points from moving.
		/// </summary>
		private readonly IReadOnlyList<TrackCircuit> ProtectingTCs;

		/// <summary>
		/// The containing world.
		/// </summary>
		private readonly World World;

		/// <summary>
		/// Whether or not any track circuit that protects these points is currently route locked.
		/// </summary>
		private bool AnyProtectingTCRouteLocked {
			get {
				foreach (TrackCircuit tc in ProtectingTCs) {
					if (tc.RouteLocked) {
						return true;
					}
				}
				return false;
			}
		}

		/// <summary>
		/// Invoked when a property changes on a protecting track circuit.
		/// </summary>
		/// <param name="sender">The track circuit.</param>
		/// <param name="e">Details of the change.</param>
		private void OnTCPropChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(TrackCircuit.RouteLock)) {
				EmitPropertyChanged(nameof(Movable));
			}
		}

		/// <summary>
		/// Configures these points for hand or power operation.
		/// </summary>
		/// <param name="value"><c>true</c> to configure for hand operation, or <c>false</c> to configure for power operation.</param>
		private async void SetHandCrankable(bool value) {
			await World.ThrowSwitchAsync(SubArea, ID, value ? ESwitchState.Unlocked : ESwitchState.Locked);
		}

		private bool HandCrankableImpl;
		private bool InconsistentImpl;
		private bool ReversedImpl;
		private bool ProvedImpl;
		private bool KeyedImpl;
		private bool OccupiedImpl;
		#endregion
	}
}
