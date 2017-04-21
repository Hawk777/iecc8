namespace Iecc8.World {
	/// <summary>
	/// Power points and their required position.
	/// </summary>
	public struct RoutePointPosition {
		#region Common API
		/// <summary>
		/// Which points.
		/// </summary>
		public readonly Points Points;

		/// <summary>
		/// Which position the points need to be in.
		/// </summary>
		public readonly bool Reverse;

		/// <summary>
		/// Whether the positions of these points are ignored when determining whether this route matches the lay of lineside equipment for aspect calculation purposes.
		/// </summary>
		public readonly bool IgnoredForAspectCalculation;
		#endregion

		#region Data Initialization API
		/// <summary>
		/// Constructs a new route point position.
		/// </summary>
		/// <param name="schema">The schema object containing data about this route point position.</param>
		/// <param name="region">The region in which the points are found.</param>
		/// <param name="subArea">The default sub-area ID against which to resolve a short points ID.</param>
		public RoutePointPosition(Schema.RoutePointPosition schema, Region region, ushort subArea) {
			Points = region.GetPowerPoints(schema.Points, subArea);
			Reverse = schema.Reversed;
			IgnoredForAspectCalculation = schema.IgnoredForAspectCalculation;
		}
		#endregion
	}
}
