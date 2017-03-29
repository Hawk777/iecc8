using System.Collections.Generic;

namespace Iecc8.Schema {
	/// <summary>
	/// Data about a sub-area.
	/// </summary>
	public class SubArea {
		/// <summary>
		/// The automatic signals in this sub-area, indexed by <c>(-id - 1)</c>.
		/// </summary>
		public List<AutomaticSignal> AutomaticSignals {
			get;
			set;
		}

		/// <summary>
		/// The controlled signals in this sub-area, indexed by ID.
		/// </summary>
		public List<ControlledSignal> ControlledSignals {
			get;
			set;
		}

		/// <summary>
		/// The track circuits in this sub-area, indexed by ID.
		/// </summary>
		public List<TrackCircuit> TrackCircuits {
			get;
			set;
		}

		/// <summary>
		/// The points in this sub-area, indexed by ID.
		/// </summary>
		public List<Points> Points {
			get;
			set;
		}

		/// <summary>
		/// The name of this sub-area, for display purposes.
		/// </summary>
		public string Name {
			get;
			set;
		}

		/// <summary>
		/// Constructs a new sub-area.
		/// </summary>
		public SubArea() {
			AutomaticSignals = new List<AutomaticSignal>();
			ControlledSignals = new List<ControlledSignal>();
			TrackCircuits = new List<TrackCircuit>();
			Points = new List<Points>();
		}
	}
}
