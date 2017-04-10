using System.Collections.Generic;

namespace Iecc8.Schema {
	/// <summary>
	/// Data about a route.
	/// </summary>
	public class Route {
		/// <summary>
		/// The ID of the exit signal.
		/// </summary>
		/// <remarks>
		/// This can be a short local signal ID or a long global signal ID.
		/// </remarks>
		public int Exit {
			get;
			set;
		}

		/// <summary>
		/// The points involved in this route and the positions they need to be in.
		/// </summary>
		public List<RoutePointPosition> Points {
			get;
			set;
		}

		/// <summary>
		/// The track circuits involved in this route and the directions they need to be locked in.
		/// </summary>
		/// <remarks>
		/// The track circuits must be listed in the order in which a train taking this route will traverse them.
		/// </remarks>
		public List<RouteTC> TCs {
			get;
			set;
		}

		/// <summary>
		/// The track circuits that must be unlocked and unoccupied in order to call this route, but are not actually part of the route.
		/// </summary>
		/// <remarks>
		/// An example of a case where this list would be needed is a crossing. Calling the north/south route requires that the east/west track circuit be free, but does not actually pass over the east/west circuit.
		/// </remarks>
		public List<ushort> FreeTCs {
			get;
			set;
		}

		/// <summary>
		/// Which divergence position this route uses (i.e. which head of the protecting signal shows its proceed aspect).
		/// </summary>
		public byte Divergence {
			get;
			set;
		}

		/// <summary>
		/// Whether this route shows up as a restricting route rather than a main route.
		/// </summary>
		public bool Restricting {
			get;
			set;
		}

		/// <summary>
		/// How this route, if diverging, calculates its aspect in terms of routes ahead.
		/// </summary>
		/// <remarks>
		/// If this field is <c>false</c> (the default), the lower aspect is calculated based on the total number of blocks ahead that are available up to the next signal at stop; for example, if the next signal shows red over yellow, then this signal will show red over flashing yellow. If this field is <c>true</c>, the lower aspect is calculated based only on the number of blocksare available up to the next signal at stop or diverging; for example, if the next signal shows red over yellow, then this signal will also show red over yellow because only one block is available until the next divergence.
		///
		/// This field is ignored unless this is a non-restricting diverging route.
		/// </remarks>
		public bool DivergenceDistanceStraightOnly {
			get;
			set;
		}

		/// <summary>
		/// Constructs a new Route.
		/// </summary>
		public Route() {
			Points = new List<RoutePointPosition>();
			TCs = new List<RouteTC>();
			FreeTCs = new List<ushort>();
			DivergenceDistanceStraightOnly = false;
		}
	}
}
