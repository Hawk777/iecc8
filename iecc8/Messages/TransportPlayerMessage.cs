using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// Where to teleport the player.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromDispatcher")]
	public struct TransportPlayerMessage {
		/// <summary>
		/// The sub-area to teleport to.
		/// </summary>
		[DataMember]
		public int Route;

		/// <summary>
		/// The power points to teleport to.
		/// </summary>
		[DataMember]
		public int SwitchIndex;

		public override string ToString() {
			return nameof(TransportPlayerMessage) + "(" + Route + ", " + SwitchIndex + ")";
		}
	}
}
