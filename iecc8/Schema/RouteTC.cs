namespace Iecc8.Schema {
	/// <summary>
	/// Data about a track circuit with respect to its position in a route.
	/// </summary>
	public struct RouteTC {
		/// <summary>
		/// Which track circuit this object refers to.
		/// </summary>
		public uint TC {
			get;
			set;
		}

		/// <summary>
		/// Which direction the track circuit is locked in when calling this route.
		/// </summary>
		public char Direction {
			get;
			set;
		}
	}
}
