using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// What level of control the external dispatcher has over lineside equipment.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromRun8")]
	public enum EDispatcherPermission {
		/// <summary>
		/// The access level has not changed.
		/// </summary>
		/// <remarks>
		/// This value appears to be unused.
		/// </remarks>
		[EnumMember]
		NoChange,

		/// <summary>
		/// The external dispatcher has full control over lineside equipment.
		/// </summary>
		[EnumMember]
		Granted,

		/// <summary>
		/// The external dispatcher has no access to any equipment.
		/// </summary>
		[EnumMember]
		Rescinded,

		/// <summary>
		/// The external dispatcher may view the state of lineside equipment but not control it.
		/// </summary>
		[EnumMember]
		Observer,
	}
}
