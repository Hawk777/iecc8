namespace Iecc8.Schema {
	/// <summary>
	/// Data about points and what position they need to be in for a route.
	/// </summary>
	public struct RoutePointPosition {
		/// <summary>
		/// The ID of the points.
		/// </summary>
		public uint Points {
			get;
			set;
		}

		/// <summary>
		/// Which position the points need to be in.
		/// </summary>
		public bool Reversed {
			get;
			set;
		}
	}
}
