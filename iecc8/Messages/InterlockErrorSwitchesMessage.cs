using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// Which power points are in inconsistent positions.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromRun8")]
	public struct InterlockErrorSwitchesMessage {
		/// <summary>
		/// Which sub-area the points are in.
		/// </summary>
		[DataMember]
		public int Route;

		/// <summary>
		/// Which power points are in inconsistent positions.
		/// </summary>
		[DataMember]
		public List<int> InterlockErrorSwitches;

		public override string ToString() {
			return nameof(InterlockErrorSwitchesMessage) + "(" + Route + ", " + InterlockErrorSwitches.Count + " items)";
		}
	}
}
