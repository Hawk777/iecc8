using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// An order to change the state of power points.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromDispatcher")]
	public struct DispatcherSwitchMessage {
		/// <summary>
		/// Which sub-area the points are in.
		/// </summary>
		[DataMember]
		public int Route;

		/// <summary>
		/// Which points to modify.
		/// </summary>
		[DataMember]
		public int SwitchID;

		/// <summary>
		/// What action the points should take.
		/// </summary>
		[DataMember]
		public ESwitchState SwitchState;

		public override string ToString() {
			return nameof(DispatcherSwitchMessage) + "(" + Route + ", " + SwitchID + ", " + SwitchState + ")";
		}
	}
}
