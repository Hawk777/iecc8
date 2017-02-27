using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// The possible states of a controlled signal.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromDispatcher")]
	public enum ESignalIndication {
		/// <summary>
		/// The signal displays a Stop indication.
		/// </summary>
		[EnumMember]
		Stop,

		/// <summary>
		/// The signal displays an indication that depends on the positions of points and occupancy of track circuits ahead as well as the indication displayed on the next signal in advance.
		/// </summary>
		[EnumMember]
		Proceed,

		/// <summary>
		/// The signal displays the same indication that it would in the Proceed state, but its state does not automatically change to Stop when a train passes.
		/// </summary>
		[EnumMember]
		Fleet,

		/// <summary>
		/// The signal displays a restricting indication.
		/// </summary>
		[EnumMember]
		FlagBy,
	}
}
