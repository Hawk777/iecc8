using System.Collections.Generic;

namespace Iecc8.Schema {
	/// <summary>
	/// Data about a controlled signal.
	/// </summary>
	public class ControlledSignal {
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
		/// The routes starting from this signal.
		/// </summary>
		public List<Route> Routes {
			get;
			set;
		}

		/// <summary>
		/// The exemptions from approach locking that are granted by comprehensive approach locking rules.
		/// </summary>
		/// <remarks>
		/// If any exemption in this list matches, approach locking is not applied when cancelling a route from this signal.
		/// </remarks>
		public List<CALExemption> CALExemptions {
			get;
			set;
		}

		/// <summary>
		/// Constructs a new controlled signal.
		/// </summary>
		public ControlledSignal() {
			Routes = new List<Route>();
			CALExemptions = new List<CALExemption>();
		}
	}
}
