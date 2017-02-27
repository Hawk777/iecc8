using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// A radio text message.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromRun8")]
	public struct RadioTextMessage {
		/// <summary>
		/// Which channel the message was received on or will be sent on.
		/// </summary>
		[DataMember(IsRequired = true)]
		public int Channel;

		/// <summary>
		/// The message.
		/// </summary>
		[DataMember(IsRequired = true)]
		public string Text;

		public override string ToString() {
			return nameof(RadioTextMessage) + "(" + Channel + ", " + Text + ")";
		}
	}
}
