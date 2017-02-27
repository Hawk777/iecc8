using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// Which power points have trains over them.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromRun8")]
	public struct OccupiedSwitchesMessage {
		/// <summary>
		/// Which sub-area the points are in.
		/// </summary>
		[DataMember]
		public int Route;

		/// <summary>
		/// Which points are occupied.
		/// </summary>
		[DataMember]
		public List<int> OccupiedSwitches;

		public override string ToString() {
			return nameof(OccupiedSwitchesMessage) + "(" + Route + ", " + OccupiedSwitches.Count + " items)";
		}
	}
}
