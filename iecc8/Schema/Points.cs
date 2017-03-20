using System.Collections.Generic;

namespace Iecc8.Schema {
	/// <summary>
	/// Data about power points.
	/// </summary>
	public class Points {
		/// <summary>
		/// The IDs of the track circuits that protect these points.
		/// </summary>
		public List<ushort> ProtectingTCs {
			get;
			set;
		}

		/// <summary>
		/// Constructs a new points.
		/// </summary>
		public Points() {
			ProtectingTCs = new List<ushort>();
		}
	}
}
