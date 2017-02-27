using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// An order to change the state of a controlled signal.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromDispatcher")]
	public struct DispatcherSignalMessage {
		/// <summary>
		/// Which sub-area the signal is in.
		/// </summary>
		[DataMember]
		public int Route;

		/// <summary>
		/// Which signal to modify.
		/// </summary>
		[DataMember]
		public int SignalID;

		/// <summary>
		/// What state to place the signal in.
		/// </summary>
		[DataMember]
		public ESignalIndication SignalIndication;

		public override string ToString() {
			return nameof(DispatcherSignalMessage) + "(" + Route + ", " + SignalID + ", " + SignalIndication + ")";
		}
	}
}
