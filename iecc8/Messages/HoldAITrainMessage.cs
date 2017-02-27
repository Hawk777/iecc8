using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// Information about which train to send orders to and whether to hold position or not.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromDispatcher")]
	public struct HoldAITrainMessage {
		/// <summary>
		/// <c>true</c> if the AI driver should brake the train and hold its position, or <c>false</c> if the AI driver should drive normally.
		/// </summary>
		[DataMember]
		public bool Hold;

		/// <summary>
		/// Which train to send orders to.
		/// </summary>
		[DataMember]
		public int TrainID;

		public override string ToString() {
			return nameof(HoldAITrainMessage) + "(" + Hold + ", " + TrainID + ")";
		}
	}
}
