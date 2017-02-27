using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// Which train an AI driver should board.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromDispatcher")]
	public struct AIRecrewTrainMessage {
		/// <summary>
		/// Which train to board.
		/// </summary>
		[DataMember]
		public int TrainID;

		public override string ToString() {
			return nameof(AIRecrewTrainMessage) + "(" + TrainID + ")";
		}
	}
}
