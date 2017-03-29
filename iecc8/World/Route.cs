using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Iecc8.World {
	/// <summary>
	/// A route.
	/// </summary>
	public class Route {
		#region Common API
		/// <summary>
		/// The signal at which this route starts.
		/// </summary>
		public readonly ControlledSignal Entrance;

		/// <summary>
		/// The signal at which this route ends.
		/// </summary>
		public readonly Signal Exit;

		/// <summary>
		/// The divergence level of this route (i.e. which head the immediate aspect for the route itself is shown on).
		/// </summary>
		public readonly byte Divergence;

		/// <summary>
		/// Whether the entrance signal displays a restricting aspect when this route is set.
		/// </summary>
		public readonly bool Restricting;

		/// <summary>
		/// The track circuits along this route.
		/// </summary>
		public readonly IReadOnlyList<RouteElement> Elements;

		/// <summary>
		/// The points along this route and what position they need to be in.
		/// </summary>
		public readonly IReadOnlyList<RoutePointPosition> PointPositions;

		/// <summary>
		/// The track circuits that need to be free in order for this route to be available, but which are not actually part of the route.
		/// </summary>
		public readonly IReadOnlyList<TrackCircuit> FreeTrackCircuits;
		#endregion

		#region Signaller API
		/// <summary>
		/// Whether this route is able to be called right now.
		/// </summary>
		public bool Available {
			get {
				// If the entrance signal already has a route and isn't approach locked, you can't set another one. If it is approach locked, you can overset exactly the same route, and this will be enforced by later code checking track circuit directions and point positions.
				if ((Entrance.CurrentRoute != null) && !Entrance.ApproachLocked) {
					return false;
				}

				// If the entrance signal is in flag-by mode, you can't set a route. You must clear the flag-by status first.
				if (Entrance.FlagBy) {
					return false;
				}

				// All occupied or locked track circuits must be locked in the same direction as this route requires it to prevent two exactly opposite routes from being called.
				foreach (RouteElement i in Elements) {
					TrackCircuit tc = i.TrackCircuit;
					if ((tc.RouteLocked || tc.Occupied) && tc.RouteLockedDirection != '\0' && tc.RouteLockedDirection != i.Direction) {
						return false;
					}
				}

				// All track circuits that are required to be free must be so.
				foreach (TrackCircuit i in FreeTrackCircuits) {
					if (i.RouteLocked || i.Occupied) {
						return false;
					}
				}

				// All points in the route must be in power-operated mode and be able to reach their required positions.
				foreach (RoutePointPosition i in PointPositions) {
					if (i.Points.HandCrankable) {
						return false;
					}
					if (!i.Points.Movable && i.Points.Reversed != i.Reverse) {
						return false;
					}
				}
				return true;
			}
		}

		/// <summary>
		/// Calls the route.
		/// </summary>
		/// <remarks>
		/// The caller must verify that the route is available first.
		/// </remarks>
		public async Task CallAsync() {
			Debug.Assert(Available);

			// Lock the track circuits.
			for (int i = 0; i != Elements.Count; ++i) {
				TrackCircuit current = Elements[i].TrackCircuit;
				TrackCircuit next = i + 1 < Elements.Count ? Elements[i + 1].TrackCircuit : null;
				current.RouteLock(Elements[i].Direction, next);
			}

			// Set the signal's route.
			Entrance.SetCurrentRoute(this);

			// Swing and lock the points.
			Task[] tasks = new Task[PointPositions.Count];
			for (int i = 0; i != PointPositions.Count; ++i) {
				tasks[i] = PointPositions[i].Points.SwingAsync(PointPositions[i].Reverse);
			}
			foreach (Task i in tasks) {
				await i;
			}
		}
		#endregion

		#region Data Initialization API
		/// <summary>
		/// Constructs a route from its spec.
		/// </summary>
		/// <param name="schema">The schema object containing data about this route.</param>
		/// <param name="region">The region the route exists in.</param>
		/// <param name="entrance">The signal at which this route begins.</param>
		public Route(Schema.Route schema, Region region, ControlledSignal entrance) {
			Entrance = entrance;
			Exit = region.GetSignal(schema.Exit, entrance.SubArea);
			Divergence = schema.Divergence;
			Restricting = schema.Restricting;
			{
				RouteElement[] elts = new RouteElement[schema.TCs.Count];
				for (int i = 0; i != schema.TCs.Count; ++i) {
					elts[i] = new RouteElement(schema.TCs[i], region, entrance.SubArea);
				}
				Elements = Array.AsReadOnly(elts);
			}
			{
				RoutePointPosition[] elts = new RoutePointPosition[schema.Points.Count];
				for (int i = 0; i != schema.Points.Count; ++i) {
					elts[i] = new RoutePointPosition(schema.Points[i], region, entrance.SubArea);
				}
				PointPositions = Array.AsReadOnly(elts);
			}
			{
				TrackCircuit[] tcs = new TrackCircuit[schema.FreeTCs.Count];
				for (int i = 0; i != schema.FreeTCs.Count; ++i) {
					tcs[i] = region.GetTrackCircuit(schema.FreeTCs[i], entrance.SubArea);
				}
				FreeTrackCircuits = Array.AsReadOnly(tcs);
			}
		}
		#endregion
	}
}
