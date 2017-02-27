using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// What type of radio tone was received by a tower.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromDispatcher")]
	public enum EDTMFType {
		/// <summary>
		/// No tone was received.
		/// </summary>
		/// <remarks>
		/// This value appears to be unused.
		/// </remarks>
		[EnumMember]
		None,

		/// <summary>
		/// A normal tone was received.
		/// </summary>
		[EnumMember]
		DispatchTone,

		/// <summary>
		/// The emergency tone *911 was received.
		/// </summary>
		[EnumMember]
		EmergencyTone,
	}
}
