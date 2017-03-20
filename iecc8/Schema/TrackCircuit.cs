namespace Iecc8.Schema {
	/// <summary>
	/// Data about a track circuit.
	/// </summary>
	public struct TrackCircuit {
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
	}
}
