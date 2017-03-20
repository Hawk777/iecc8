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
		/// Constructs a new Route.
		/// </summary>
		public Route() {
			Points = new List<RoutePointPosition>();
			TCs = new List<RouteTC>();
			FreeTCs = new List<ushort>();
		}
	}
}
