using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// The possible types of driver that can be sitting in a train.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromRun8")]
	public enum EEngineerType {
		/// <summary>
		/// The driver's seat is empty.
		/// </summary>
		[EnumMember]
		None,

		/// <summary>
		/// A human is in control.
		/// </summary>
		[EnumMember]
		Player,

		/// <summary>
		/// An AI is in control.
		/// </summary>
		[EnumMember]
		AI,
	}
}
