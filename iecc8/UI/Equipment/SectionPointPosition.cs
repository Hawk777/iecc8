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
	}
}
