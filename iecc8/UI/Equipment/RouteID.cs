namespace Iecc8.UI.Equipment {
	/// <summary>
	/// The identity of a particular route.
	/// </summary>
	public struct RouteID {
		/// <summary>
		/// The entrance signal number.
		/// </summary>
		public int Entrance {
			get;
			set;
		}

		/// <summary>
		/// The exit signal number.
		/// </summary>
		public int Exit {
			get;
			set;
		}

		/// <summary>
		/// Constructs a new RouteID with all values filled in.
		/// </summary>
		/// <param name="entrance">The entrance signal number.</param>
		/// <param name="exit">The exit signal number.</param>
		public RouteID(int entrance, int exit) {
			Entrance = entrance;
			Exit = exit;
		}
	}
}
