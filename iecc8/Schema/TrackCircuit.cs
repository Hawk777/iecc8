namespace Iecc8.Schema {
	/// <summary>
	/// Data about a track circuit.
	/// </summary>
	public struct TrackCircuit {
		/// <summary>
		/// The ID number of the track circuit.
		/// </summary>
		public ushort ID {
			get;
			set;
		}

		/// <summary>
		/// Whether to ignore reversed hand points in this track circuit.
		/// </summary>
		/// <remarks>
		/// Some track circuits are reported by Run8 as though they are wired into hand points, but those hand points should be ignored for the purpose of reporting TC occupancy and route availability. An example is when the hand points are behind power points which separate them from the mainline.
		/// </remarks>
		public bool IgnoreHandPoints {
			get;
			set;
		}

		/// <summary>
		/// The name of this location.
		/// </summary>
		/// <remarks>
		/// This is used only for display when trying to locate a train. It need not be entirely unique across track circuits; a handful of track circuits in the same area can (and probably should) share a single location name. When displayed, it will be prefixed with the sub-area name.
		/// </remarks>
		public string LocationName {
			get;
			set;
		}
	}
}
