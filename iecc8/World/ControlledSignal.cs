using Iecc8.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Iecc8.World {
	/// <summary>
	/// A controlled signal.
	/// </summary>
	public class ControlledSignal : Signal {
		#region Common API
		/// <summary>
		/// The set of all routes entered at this signal, indexed by exit signal.
		/// </summary>
		public IReadOnlyDictionary<Signal, Route> RoutesFrom {
			get; private set;
		}

		/// <summary>
		/// Which route is currently set from this signal, or <c>null</c> if no route is set.
		/// </summary>
		public Route CurrentRoute {
			get {
				return CurrentRouteImpl;
			}
			private set {
				if (SetProperty(ref CurrentRouteImpl, value)) {
					EmitPropertyChanged(nameof(AutoWorkingAvailable));
				}
			}
		}

		/// <summary>
		/// Whether this signal is automatically worked.
		/// </summary>
		public bool AutoWorking {
			get {
				return AutoWorkingImpl;
			}
			private set {
				SetProperty(ref AutoWorkingImpl, value);
			}
		}

		/// <summary>
		/// Whether it is possible to enable automatic working.
		/// </summary>
		public bool AutoWorkingAvailable {
			get {
				// This is roughly equivalent to whether or not a route is set. However, to avoid a race condition where Run8 replaces the signal just as the signaller enables automatic working (thus resulting in a fleeted signal without a route), we stop making automatic working available as soon as the replace delay timer starts counting down, even if the route has not yet released.
				return (CurrentRoute != null) && (ReplaceDelayTimer == ReplaceDelayPeriod) && !SwingingPoints;
			}
		}

		/// <summary>
		/// Whether this signal is in flag-by mode.
		/// </summary>
		public bool FlagBy {
			get {
				return FlagByImpl;
			}
			private set {
				if (SetProperty(ref FlagByImpl, value)) {
					UpdateAspects();
				}
			}
		}

		/// <summary>
		/// Whether this signal is waiting for approach locking to time out.
		/// </summary>
		public bool ApproachLocked {
			get {
				return ApproachLockedImpl;
			}
			private set {
				SetProperty(ref ApproachLockedImpl, value);
			}
		}
		#endregion

		#region Signaller API
		/// <summary>
		/// Enables flag-by mode.
		/// </summary>
		/// <remarks>
		/// The signal must not have a route set.
		/// </remarks>
		public Task EnableFlagByAsync() {
			Debug.Assert(CurrentRoute == null);
			return World.ChangeSignalAsync(SubArea, ID, ESignalIndication.FlagBy);
		}

		/// <summary>
		/// Cancels the route set from this signal, if any.
		/// </summary>
		public Task CancelAsync() {
			// Consider applying approach locking. Apply it if a route is locked, approach locking isn't already in progress, the signal is off, and comprehensive approach locking doesn't exempt us.
			if ((CurrentRoute != null) && !ApproachLocked && Aspects.Off) {
				bool exempt = false;
				foreach (CALExemption i in CALExemptions) {
					exempt |= i.Check();
				}
				if (!exempt) {
					ApproachLockExpires = DateTime.UtcNow.AddSeconds(ApproachLockingTime);
					ApproachLocked = true;
				}
			}

			// Remember that we are no longer waiting for points to prove. If we left this set to true, we could get a race condition where we cancel the route, then the points reach their final positions, and then we change the indication back to proceed. Any subsequent overset will bring this back to true, so setting it to false here is OK.
			SwingingPoints = false;

			// Change the signal. This will eventually trickle back through an update packet to cause "TORR" (not actually train-operated) and drop the route.
			return World.ChangeSignalAsync(SubArea, ID, ESignalIndication.Stop);
		}
		#endregion

		#region Data Initialization API
		/// <summary>
		/// Constructs a controlled signal.
		/// </summary>
		/// <param name="schema">The schema object containing data about this signal.</param>
		/// <param name="id">The internal Run8 ID number.</param>
		/// <param name="subArea">The ID of the sub-area that contains this signal.</param>
		/// <param name="world">The containing world.</param>
		public ControlledSignal(Schema.ControlledSignal schema, ushort id, ushort subArea, World world) : base(subArea, (short) id, schema.HeadCount, schema.DivergenceLookahead) {
			Debug.Assert(world != null);
			World = world;
		}

		/// <summary>
		/// Connects this signal to other lineside equipment.
		/// </summary>
		/// <remarks>
		/// This must only be called by the SubArea constructor.
		/// </remarks>
		/// <param name="schema">The schema object containing data about this signal.</param>
		/// <param name="region">The region that contains this signal.</param>
		public void InitLinks(Schema.ControlledSignal schema, Region region) {
			{
				Dictionary<Signal, Route> routesFrom = new Dictionary<Signal, Route>();
				foreach (Schema.Route i in schema.Routes) {
					Route r = new Route(i, region, this);
					routesFrom[r.Exit] = r;
				}
				RoutesFrom = new ReadOnlyDictionary<Signal, Route>(routesFrom);
			}

			ISet<Signal> allExitSignals = new HashSet<Signal>();
			ISet<Points> allPoints = new HashSet<Points>();
			ISet<TrackCircuit> allTCs = new HashSet<TrackCircuit>();
			foreach (KeyValuePair<Signal, Route> i in RoutesFrom) {
				allExitSignals.Add(i.Key);
				foreach (RoutePointPosition j in i.Value.PointPositions) {
					allPoints.Add(j.Points);
				}
				foreach (RouteElement j in i.Value.Elements) {
					allTCs.Add(j.TrackCircuit);
				}
			}
			foreach (Signal i in allExitSignals) {
				i.PropertyChanged += OnNextSignalPropChanged;
			}
			foreach (Points i in allPoints) {
				i.PropertyChanged += OnPointsPropChanged;
			}
			foreach (TrackCircuit i in allTCs) {
				i.PropertyChanged += OnTCPropChanged;
			}

			{
				CALExemption[] ary = new CALExemption[schema.CALExemptions.Count];
				for (int i = 0; i != schema.CALExemptions.Count; ++i) {
					ary[i] = new CALExemption(schema.CALExemptions[i], region, SubArea);
				}
				CALExemptions = Array.AsReadOnly(ary);
			}
		}
		#endregion

		#region Run8 API
		/// <summary>
		/// Processes a message from Run8 carrying the indication of this signal.
		/// </summary>
		/// <param name="indication">This signal's indication.</param>
		public async Task UpdateFromRun8Async(ESignalIndication indication) {
			// Update indication.
			if (LastIndication != indication) {
				LastIndication = indication;
				UpdateAspects();
			}

			// Calculate a few derived values.
			AutoWorking = indication == ESignalIndication.Fleet;
			FlagBy = indication == ESignalIndication.FlagBy;

			// Clear approach locking if the signal is set to proceed. Approach locking will be reapplied if the signal is cancelled a second time. The only cases where approach locking would not be reapplied would be (1) if a CAL exemption matches, in which case the old approach locking is clearly no longer needed, or (2) if the signal is on, which can only happen if a TC in advance of the signal is occupied, which means either the approaching train passed the signal (in which case approach locking should no longer apply and TORR should be allowed to happen) or some other train approaching the area SPADed (in which case all bets are off and it doesn't really matter what we do).
			if ((indication != ESignalIndication.Stop) && (indication != ESignalIndication.FlagBy)) {
				ApproachLockExpires = null;
			}

			// Update approach locking status based on timer.
			ApproachLocked = ApproachLockExpires.HasValue && (ApproachLockExpires.Value > DateTime.UtcNow);

			// Check for train operated route release.
			if (!SwingingPoints && (indication == ESignalIndication.Stop) && !ApproachLocked) {
				// We aren't waiting for points before clearing a signal, the signal is at stop, and we aren't waiting for approach locking to time out. That means either there is no route set (in which case all this logic is harmless), a route is set and the train passing replaced the signal, or the signal was cancelled and either was exempt from approach locking or the timeout has passed.
				if (ReplaceDelayTimer == 0) {
					if (CurrentRoute != null) {
						CurrentRoute.Elements[0].TrackCircuit.RouteLockCascaded = false;
						CurrentRoute = null;
					}
				} else {
					--ReplaceDelayTimer;
				}
			} else {
				ReplaceDelayTimer = ReplaceDelayPeriod;
			}

			// If we were waiting for the points to prove after calling a route and they have now done so, clear the signal.
			if (SwingingPoints) {
				bool anyWrong = false;
				foreach (RoutePointPosition i in CurrentRoute.PointPositions) {
					if (!i.Points.Proved) {
						anyWrong = true;
					}
				}
				if (!anyWrong) {
					SwingingPoints = false;
					ReplaceDelayTimer = ReplaceDelayPeriod;
					await World.ChangeSignalAsync(SubArea, ID, AutoWorking ? ESignalIndication.Fleet : ESignalIndication.Proceed);
				}
			}
		}
		#endregion

		#region Interlocking Internal API
		/// <summary>
		/// Sets the current route from this signal.
		/// </summary>
		/// <param name="route">The route.</param>
		public void SetCurrentRoute(Route route) {
			Debug.Assert(route != null);
			CurrentRoute = route;
			SwingingPoints = true;
			ReplaceDelayTimer = ReplaceDelayPeriod;
		}
		#endregion

		#region Private Members
		/// <summary>
		/// The containing world.
		/// </summary>
		private readonly World World;

		/// <summary>
		/// How many updates need to arrive with the signal on before any route previously set from it is dropped.
		/// </summary>
		/// <remarks>
		/// A delay timer is needed because signal indication changes and track circuit occupancy are carried in different messages from Run8. It is therefore possible that, as a train passes a signal, the signal could be reported as being replaced before the first track circuit is reported as occupied. Should that happen, we would not want route locking to drop. Therefore, we wait until a few updates later, giving plenty of time for the track circuit to occupy first.
		/// </remarks>
		private const byte ReplaceDelayPeriod = 2;

		/// <summary>
		/// How many seconds of approach locking apply.
		/// </summary>
		private const ushort ApproachLockingTime = 120;

		/// <summary>
		/// How many updates are left on the replacement delay timer before the route can be dropped.
		/// </summary>
		private byte ReplaceDelayTimer {
			get {
				return ReplaceDelayTimerImpl;
			}
			set {
				// This is private so no notification of itself is needed; however, AutoWorkingAvailable may be changed as a side effect.
				SetProperty(ref ReplaceDelayTimerImpl, value, nameof(AutoWorkingAvailable));
			}
		}
		private byte ReplaceDelayTimerImpl = 0;

		/// <summary>
		/// Whether the route set from this signal is waiting for points to swing to their proper positions.
		/// </summary>
		private bool SwingingPoints;

		private Route CurrentRouteImpl;
		private bool AutoWorkingImpl;
		private bool FlagByImpl;
		private bool ApproachLockedImpl;
		private ESignalIndication LastIndication;
		private IReadOnlyList<CALExemption> CALExemptions;
		private DateTime? ApproachLockExpires;

		/// <summary>
		/// Invoked when a property changes on one of the possible next signals.
		/// </summary>
		/// <param name="sender">The signal.</param>
		/// <param name="e">Details about the property change.</param>
		private void OnNextSignalPropChanged(object sender, PropertyChangedEventArgs e) {
			UpdateAspects();
		}

		/// <summary>
		/// Invoked when a property changes on one of the points this signal may be protecting.
		/// </summary>
		/// <param name="sender">The points.</param>
		/// <param name="e">Details about the property change.</param>
		private void OnPointsPropChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(Points.Reversed)) {
				UpdateAspects();
			}
		}

		/// <summary>
		/// Invoked when a property changes on one of the track circuits this signal may be protecting.
		/// </summary>
		/// <param name="sender">The track circuit.</param>
		/// <param name="e">Details about the property change.</param>
		private void OnTCPropChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(TrackCircuit.Occupied) || e.PropertyName == nameof(TrackCircuit.ReversedHandPoints)) {
				UpdateAspects();
			}
		}

		/// <summary>
		/// Calculates and sets new signal aspects.
		/// </summary>
		private void UpdateAspects() {
			if (LastIndication == ESignalIndication.Stop) {
				Aspects = new Aspects(EAspectsType.Red, 0);
			} else if (FlagBy) {
				Aspects = new Aspects(EAspectsType.AllLunar, 0);
			} else {
				Route route = null;
				foreach (KeyValuePair<Signal, Route> i in RoutesFrom) {
					bool matches = true;
					foreach (RoutePointPosition j in i.Value.PointPositions) {
						matches = matches && (j.Points.Reversed == j.Reverse);
					}
					if (matches) {
						route = i.Value;
						break;
					}
				}
				if (route == null) {
					Aspects = new Aspects(EAspectsType.Red, 0);
				} else {
					bool tcUnclear = false;
					foreach (RouteElement i in route.Elements) {
						tcUnclear = tcUnclear || i.TrackCircuit.Occupied || i.TrackCircuit.ReversedHandPoints;
					}
					if (tcUnclear) {
						Aspects = new Aspects(EAspectsType.Red, 0);
					} else if (route.Restricting) {
						Aspects = new Aspects(EAspectsType.OneLunar, route.Divergence);
					} else if (route.Divergence == 0) {
						Aspects = route.Exit.Aspects.NextInRear(DivergenceLookahead);
					} else {
						Aspects = route.Exit.Aspects.NextInRearDiverging(route.Divergence, route.DivergenceDistanceStraightOnly);
					}
				}
			}
		}
		#endregion
	}
}
