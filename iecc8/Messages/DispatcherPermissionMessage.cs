using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// Information about what level of access the external dispatcher has.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromRun8")]
	public struct DispatcherPermissionMessage {
		/// <summary>
		/// Whether or not this player is able to issue orders to AI drivers.
		/// </summary>
		[DataMember]
		public bool AIPermission;

		/// <summary>
		/// What level of control the external dispatcher has over lineside equipment.
		/// </summary>
		[DataMember]
		public EDispatcherPermission Permission;

		override public string ToString() {
			return nameof(DispatcherPermissionMessage) + "(" + Permission + ", " + AIPermission + ")";
		}
	}
}
