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

		/// <summary>
		/// Whether the positions of these points are ignored when determining whether this route matches the lay of lineside equipment for aspect calculation purposes.
		/// </summary>
		public bool IgnoredForAspectCalculation {
			get;
			set;
		}
	}
}
