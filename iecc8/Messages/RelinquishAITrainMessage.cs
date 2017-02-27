using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// Information about which train to send orders to and whether the AI driver should disembark or not.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromDispatcher")]
	public struct RelinquishAITrainMessage {
		/// <summary>
		/// <c>true</c> if the AI driver should disembark the train once it has stopped, or <c>false</c> if the driver should remain on board.
		/// </summary>
		[DataMember]
		public bool RelinquishAITrainWhenStopped;

		/// <summary>
		/// Which train to send orders to.
		/// </summary>
		[DataMember]
		public int TrainID;

		public override string ToString() {
			return nameof(RelinquishAITrainMessage) + "(" + RelinquishAITrainWhenStopped + ", " + TrainID + ")";
		}
	}
}
