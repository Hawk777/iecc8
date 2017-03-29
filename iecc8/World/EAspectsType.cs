namespace Iecc8.World {
	/// <summary>
	/// All the possible combinations of aspects a signal can exhibit.
	/// </summary>
	/// <remarks>
	/// Some of the entries in this enumeration refer to an "identified head". This is identified by a separate number which must be passed alongside.
	/// </remarks>
	public enum EAspectsType {
		/// <summary>
		/// All heads are red.
		/// </summary>
		Red,

		/// <summary>
		/// All heads are lunar.
		/// </summary>
		AllLunar,

		/// <summary>
		/// Identified head is lunar; all others are red.
		/// </summary>
		OneLunar,

		/// <summary>
		/// Top head is yellow; all others are red.
		/// </summary>
		Yellow,

		/// <summary>
		/// Top head is flashing yellow; all others are red.
		/// </summary>
		FlashingYellow,

		/// <summary>
		/// Top head is flashing green; all others are red.
		/// </summary>
		Green,

		/// <summary>
		/// Top head is red; identified head is yellow.
		/// </summary>
		RedOverYellow,

		/// <summary>
		/// Top head is red; identified head is flashing yellow; all others are red.
		/// </summary>
		RedOverFlashingYellow,

		/// <summary>
		/// Top head is red; identified head is green; all others are red.
		/// </summary>
		RedOverGreen,

		/// <summary>
		/// Top head and identified head are yellow; all others are red.
		/// </summary>
		YellowOverYellow,

		/// <summary>
		/// Top head is yellow; identified head is green; all others are red.
		/// </summary>
		YellowOverGreen,
	}
}
