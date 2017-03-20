using System.Collections.Generic;

namespace Iecc8.Schema {
	/// <summary>
	/// Data about a region.
	/// </summary>
	public class Region {
		/// <summary>
		/// The name of the region.
		/// </summary>
		/// <remarks>
		/// This must match a directory in the Region directory.
		/// </remarks>
		public string Name {
			get;
			set;
		}

		/// <summary>
		/// The sub-areas in the region.
		/// </summary>
		public List<RegionEntry> SubAreas {
			get;
			set;
		}

		/// <summary>
		/// Constructs a new region.
		/// </summary>
		public Region() {
			SubAreas = new List<RegionEntry>();
		}
	}
}
