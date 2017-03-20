namespace Iecc8.Schema {
	/// <summary>
	/// How far ahead a signal might look for a divergence.
	/// </summary>
	public enum EDivergenceLookahead {
		/// <summary>
		/// The signal will look one signal ahead for a divergence.
		/// </summary>
		/// <remarks>
		/// The signal will show red-over-proceed for a divergence at itself and either yellow-over-yellow or yellow-over-green for a divergence at the next signal.
		/// </remarks>
		Normal,

		/// <summary>
		/// The signal will not look ahead for a divergence.
		/// </summary>
		/// <remarks>
		/// The signal will show red-over-proceed for a divergence at itself and will otherwise show proceed-over-red.
		/// </remarks>
		Short,

		/// <summary>
		/// The signal will look two signals ahead for a divergence.
		/// </summary>
		/// <remarks>
		/// In addition to the behaviour of Normal, the signal will show flashing yellow instead of green if the next signal is yellow-over-yellow or yellow-over-green.
		/// </remarks>
		Long,
	}
}
