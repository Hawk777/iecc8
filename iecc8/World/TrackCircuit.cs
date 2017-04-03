using System.Diagnostics;

namespace Iecc8.World {
	/// <summary>
	/// A track circuit.
	/// </summary>
	public class TrackCircuit : BindableBase {
		#region Common API
		/// <summary>
		/// The sub-area that contains this track circuit.
		/// </summary>
		public readonly ushort SubArea;

		/// <summary>
		/// The Run8 internal ID number of this track circuit.
		/// </summary>
		public readonly ushort ID;

		/// <summary>
		/// The name of the location of this track circuit.
		/// </summary>
		public readonly string LocationName;

		/// <summary>
		/// The name of this track circuit.
		/// </summary>
		public string Name {
			get {
				return SubArea + "/" + ID;
			}
		}

		/// <summary>
		/// Whether the track circuit is locked by a route set from a signal.
		/// </summary>
		public bool RouteLocked {
			get {
				return RouteLockedImpl;
			}
			private set {
				if (SetProperty(ref RouteLockedImpl, value) && !value) {
					RouteLockedDirection = '\0';
					if (NextInRoute != null) {
						TrackCircuit next = NextInRoute;
						NextInRoute = null;
						next.RouteLockCascaded = false;
					}
				}
			}
		}

		/// <summary>
		/// The direction in which the track circuit is or most recently was locked.
		/// </summary>
		public char RouteLockedDirection {
			get {
				return RouteLockedDirectionImpl;
			}
			private set {
				SetProperty(ref RouteLockedDirectionImpl, value);
			}
		}

		/// <summary>
		/// Whether the track circuit is occupied by a train.
		/// </summary>
		public bool Occupied {
			get {
				return OccupiedImpl;
			}
			private set {
				if (SetProperty(ref OccupiedImpl, value)) {
					MaybeUnlock();
				}
			}
		}

		/// <summary>
		/// Whether any hand points in the track circuit are reversed.
		/// </summary>
		public bool ReversedHandPoints {
			get {
				return ReversedHandPointsImpl;
			}
			private set {
				SetProperty(ref ReversedHandPointsImpl, value);
			}
		}
		#endregion

		#region Run8 API
		/// <summary>
		/// Processes a message from Run8 carrying occupancy information for this track circuit.
		/// </summary>
		/// <param name="occupied">Whether this track circuit is occupied.</param>
		/// <param name="reversedHandPoints">Whether any hand points in the track circuit are reversed.</param>
		public void UpdateFromRun8(bool occupied, bool reversedHandPoints) {
			if (occupied) {
				LossOfShuntTimer = LossOfShuntPeriod;
				Occupied = true;
			} else {
				if (LossOfShuntTimer != 0) {
					--LossOfShuntTimer;
				} else {
					Occupied = false;
				}
			}

			ReversedHandPoints = reversedHandPoints && !IgnoreHandPoints;
		}
		#endregion

		#region Data Initialization API
		/// <summary>
		/// Constructs a new track circuit.
		/// </summary>
		/// <param name="schema">The schema object containing data about this track circuit.</param>
		/// <param name="subArea">The ID number of the sub-area that contains this track circuit.</param>
		/// <param name="id">The Run8 internal ID number of this track circuit.</param>
		public TrackCircuit(Schema.TrackCircuit schema, ushort subArea, ushort id) {
			SubArea = subArea;
			ID = id;
			LocationName = string.IsNullOrEmpty(schema.LocationName) ? string.Empty : string.Intern(schema.LocationName);
			IgnoreHandPoints = schema.IgnoreHandPoints;
		}
		#endregion

		#region Interlocking Internal API
		/// <summary>
		/// Whether the track circuit is held locked by an object in rear (another track circuit which is route locked or a signal which has a route set).
		/// </summary>
		public bool RouteLockCascaded {
			get {
				return RouteLockCascadedImpl;
			}
			set {
				if (SetProperty(ref RouteLockCascadedImpl, value)) {
					MaybeUnlock();
				}
			}
		}

		/// <summary>
		/// Locks this track circuit as part of a route.
		/// </summary>
		/// <param name="direction">The direction in which the track circuit is to be locked.</param>
		/// <param name="next">The next track circuit in the route, <c>null</c> if this is the last track circuit.</param>
		public void RouteLock(char direction, TrackCircuit next) {
			Debug.Assert(!RouteLocked || RouteLockedDirection == direction);
			RouteLockedDirection = direction;
			NextInRoute = next;
			RouteLocked = true;
			RouteLockCascaded = true;
		}

		/// <summary>
		/// Finds the berth track circuit at the end of the route running through this circuit.
		/// </summary>
		/// <returns>The circuit at the end of the route, or this circuit if there is no route.</returns>
		public TrackCircuit GetBerth() {
			return (NextInRoute != null) ? NextInRoute.GetBerth() : this;
		}
		#endregion

		#region Private Members
		/// <summary>
		/// Whether this track circuit ignores hand points.
		/// </summary>
		private readonly bool IgnoreHandPoints;

		/// <summary>
		/// The next track circuit in the route, or <c>null</c> if this is the last track circuit or no route is set.
		/// </summary>
		private TrackCircuit NextInRoute = null;

		/// <summary>
		/// Storage for the Occupied property.
		/// </summary>
		private bool OccupiedImpl = true;

		/// <summary>
		/// Storage for the RouteLocked property.
		/// </summary>
		private bool RouteLockedImpl = false;

		/// <summary>
		/// Storage for the RouteLockCascaded property.
		/// </summary>
		private bool RouteLockCascadedImpl = false;

		/// <summary>
		/// How many times Run8 must send an update reporting the track circuit as clear before we consider it to be so.
		/// </summary>
		/// <remarks>
		/// A loss of shunt timer is needed because track circuit occupancy for different sub-areas is sent in different messages. As such, it is possible that Run8 could report a track circuit as clear before reporting the next circuit in advance as occupied. Should this occur, we would not want route locking to drop. A loss of shunt timer prevents this by giving enough time for the other sub-area's update to arrive before considering the circuit in rear to be clear.
		/// </remarks>
		private const byte LossOfShuntPeriod = 2;

		/// <summary>
		/// How many updates are left on the loss-of-shunt timer before the track circuit can display as clear.
		/// </summary>
		private byte LossOfShuntTimer = LossOfShuntPeriod;

		/// <summary>
		/// Checks whether route locking should be dropped on this track circuit.
		/// </summary>
		private void MaybeUnlock() {
			if (!RouteLockCascaded && !Occupied) {
				RouteLocked = false;
			}
		}

		private char RouteLockedDirectionImpl;
		private bool ReversedHandPointsImpl;
		#endregion
	}
}
