using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// Which power points are in the reverse position.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromRun8")]
	public struct ReversedSwitchesMessage {
		/// <summary>
		/// Which sub-area the points are in.
		/// </summary>
		[DataMember]
		public int Route;

		/// <summary>
		/// Which points are in the reverse position.
		/// </summary>
		/// <remarks>
		/// This includes both locked points which are power-driven reverse and unlocked points which are hand-cranked reverse. Unlocked points in inconsistent positions may or may not appear here.
		/// </remarks>
		[DataMember]
		public List<int> ReversedSwitches;

		public override string ToString() {
			return nameof(ReversedSwitchesMessage) + "(" + Route + ", {" + string.Join(", ", ReversedSwitches) + "})";
		}
	}
}
