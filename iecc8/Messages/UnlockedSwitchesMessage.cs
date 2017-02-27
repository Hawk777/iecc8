using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// Which power points are unlocked for hand cranking.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromRun8")]
	public struct UnlockedSwitchesMessage {
		/// <summary>
		/// Which sub-area the points are in.
		/// </summary>
		[DataMember]
		public int Route;

		/// <summary>
		/// Which points are unlocked.
		/// </summary>
		[DataMember]
		public List<int> UnlockedSwitches;

		public override string ToString() {
			return nameof(UnlockedSwitchesMessage) + "(" + Route + ", " + UnlockedSwitches.Count + " items)";
		}
	}
}
