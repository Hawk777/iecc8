using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// Where to teleport the player.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromDispatcher")]
	public struct TransportPlayerToBlockMessage {
		/// <summary>
		/// The sub-area to teleport to.
		/// </summary>
		[DataMember]
		public int Route;

		/// <summary>
		/// The track circuit to teleport to.
		/// </summary>
		[DataMember]
		public int BlockDetectorID;

		public override string ToString() {
			return nameof(TransportPlayerToBlockMessage) + "(" + Route + ", " + BlockDetectorID + ")";
		}
	}
}
