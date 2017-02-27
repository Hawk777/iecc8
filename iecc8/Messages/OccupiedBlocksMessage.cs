using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// Which track circuits are occupied or have reversed hand points.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromRun8")]
	public struct OccupiedBlocksMessage {
		/// <summary>
		/// Which sub-area the track circuits are in.
		/// </summary>
		[DataMember]
		public int Route;

		/// <summary>
		/// Which track circuits are occupied.
		/// </summary>
		[DataMember]
		public List<int> OccupiedBlocks;

		/// <summary>
		/// Which track circuits have reversed hand points.
		/// </summary>
		[DataMember]
		public List<int> OpenManualSwitchBlocks;

		public override string ToString() {
			return nameof(OccupiedBlocksMessage) + "(" + Route + ", " + OccupiedBlocks.Count + " items, " + OpenManualSwitchBlocks.Count + " items)";
		}
	}
}
