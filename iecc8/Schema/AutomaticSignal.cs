namespace Iecc8.Schema {
	/// <summary>
	/// Data about an automatic signal.
	/// </summary>
	public struct AutomaticSignal {
		/// <summary>
		/// The number of heads on this signal.
		/// </summary>
		public byte HeadCount {
			get;
			set;
		}

		/// <summary>
		/// How many signals ahead this signal looks for a divergence to report.
		/// </summary>
		public EDivergenceLookahead DivergenceLookahead {
			get;
			set;
		}

		/// <summary>
		/// The track circuit that this signal protects, or <c>null</c> for none.
		/// </summary>
		public ushort? ProtectedTC {
			get;
			set;
		}

		/// <summary>
		/// The ID number of the next signal.
		/// </summary>
		/// <remarks>
		/// This can be a short local signal ID or a long global signal ID.
		/// </remarks>
		public int? NextSignal {
			get;
			set;
		}
	}
}
