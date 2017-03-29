namespace Iecc8.World {
	/// <summary>
	/// An element of a route.
	/// </summary>
	public struct RouteElement {
		#region Common API
		/// <summary>
		/// The track circuit covering this route element.
		/// </summary>
		public readonly TrackCircuit TrackCircuit;

		/// <summary>
		/// The direction in which the track circuit must be locked.
		/// </summary>
		public readonly char Direction;
		#endregion

		#region Data Initialization API
		/// <summary>
		/// Constructs a new route element.
		/// </summary>
		/// <param name="schema">The schema object containing data about this route.</param>
		/// <param name="region">The region the route exists in.</param>
		/// <param name="subArea">The default sub-area ID against which to resolve a short TC ID.</param>
		public RouteElement(Schema.RouteTC schema, Region region, ushort subArea) {
			TrackCircuit = region.GetTrackCircuit(schema.TC, subArea);
			Direction = schema.Direction;
		}
		#endregion
	}
}
