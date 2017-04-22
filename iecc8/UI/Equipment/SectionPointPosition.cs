namespace Iecc8.UI.Equipment {
	/// <summary>
	/// A combination of points ID and position.
	/// </summary>
	public struct SectionPointPosition {
		/// <summary>
		/// The points ID.
		/// </summary>
		public int Points {
			get;
			set;
		}

		/// <summary>
		/// The position.
		/// </summary>
		public bool Reversed {
			get;
			set;
		}

		/// <summary>
		/// Constructs a new SectionPointPosition with all values filled in.
		/// </summary>
		/// <param name="points">The points ID.</param>
		/// <param name="reversed">The position.</param>
		public SectionPointPosition(int points, bool reversed) {
			Points = points;
			Reversed = reversed;
		}
	}
}
