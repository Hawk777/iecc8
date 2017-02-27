using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// Information about a radio tone received by a tower.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromRun8")]
	public struct DTMFMessage {
		/// <summary>
		/// What channel the calling driver's radio was tuned to when they sent the tone.
		/// </summary>
		/// <remarks>
		/// The tone will only be received if the driver's radio is tuned to channel zero or a channel appropriate to the location.
		/// </remarks>
		[DataMember(IsRequired = false)]
		public int Channel;

		/// <summary>
		/// What type of tone was issued.
		/// </summary>
		[DataMember(IsRequired = false)]
		public EDTMFType DTMFType;

		/// <summary>
		/// The number dialed on the radio to generate the tone, including preceding *.
		/// </summary>
		[DataMember(IsRequired = false)]
		public string Tone;

		/// <summary>
		/// The name of the tower that received the tone.
		/// </summary>
		[DataMember(IsRequired = false)]
		public string TowerDescription;

		public override string ToString() {
			return nameof(DTMFMessage) + "(" + Channel + ", " + DTMFType + ", " + Tone + ", " + TowerDescription + ")";
		}
	}
}
