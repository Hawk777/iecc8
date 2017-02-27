using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// Which train to instantly stop.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromDispatcher")]
	public struct StopAITrainMessage {
		/// <summary>
		/// Which train to stop.
		/// </summary>
		[DataMember]
		public int TrainID;

		public override string ToString() {
			return nameof(StopAITrainMessage) + "(" + TrainID + ")";
		}
	}
}
