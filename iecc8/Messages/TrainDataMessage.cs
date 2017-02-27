using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// Information about a train.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromRun8")]
	public struct TrainDataMessage {
		/// <summary>
		/// The information.
		/// </summary>
		[DataMember(IsRequired = true)]
		public TrainData Train;

		public override string ToString() {
			return Train.ToString();
		}
	}
}
