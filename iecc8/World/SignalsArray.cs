namespace Iecc8.World {
	/// <summary>
	/// Allows access to any signal by ID.
	/// </summary>
	public class SignalsArray {
		#region Common API
		/// <summary>
		/// Accesses a signal.
		/// </summary>
		/// <param name="id">The ID of the signal, which must be nonnegative for a controlled signal or negative for an automatic signal.</param>
		/// <returns>The signal.</returns>
		public Signal this[short id] {
			get {
				if (id >= 0) {
					return SubArea.ControlledSignals[id];
				} else {
					return SubArea.AutomaticSignals[-id - 1];
				}
			}
		}
		#endregion

		#region Data Initialization API
		/// <summary>
		/// Constructs a SignalsArray.
		/// </summary>
		/// <param name="subArea">The sub-area accessed by this object.</param>
		public SignalsArray(SubArea subArea) {
			SubArea = subArea;
		}
		#endregion

		#region Private Members
		/// <summary>
		/// The sub-area accessed by this object.
		/// </summary>
		private readonly SubArea SubArea;
		#endregion
	}
}
