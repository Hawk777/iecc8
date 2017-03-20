using System.Collections.Generic;

namespace Iecc8.Schema {
	/// <summary>
	/// An exemption from approach locking granted by comprehensive approach locking rules.
	/// </summary>
	public class CALExemption {
		/// <summary>
		/// The track circuits that must all be clear in order for this exemption to apply.
		/// </summary>
		public List<uint> ClearTCs {
			get;
			set;
		}

		/// <summary>
		/// The points that must all be in particular designated positions in order for this exemption to apply.
		/// </summary>
		public List<RoutePointPosition> PointPositions {
			get;
			set;
		}

		/// <summary>
		/// The signals that must all be exhibiting either stop or restricting indications in order for this exemption to apply.
		/// </summary>
		public List<int> SignalsOn {
			get;
			set;
		}

		/// <summary>
		/// Constructs a new CALExemption.
		/// </summary>
		public CALExemption() {
			ClearTCs = new List<uint>();
			PointPositions = new List<RoutePointPosition>();
			SignalsOn = new List<int>();
		}
	}
}
