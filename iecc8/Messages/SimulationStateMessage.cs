using System;
using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// The in-game date and time and whether this instance of Run8 is a multiplayer client connected to a server.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromRun8")]
	public struct SimulationStateMessage {
		/// <summary>
		/// <c>true</c> if this instance of Run8 is connected to a multiplayer server, or <c>false</c> if this instance is itself a server or is playing in single-player.
		/// </summary>
		[DataMember(IsRequired = true)]
		public bool IsClient;

		/// <summary>
		/// The current date and time in the game world.
		/// </summary>
		[DataMember(IsRequired = true)]
		public DateTime SimulationTime;

		public override string ToString() {
			return nameof(SimulationStateMessage) + "(" + IsClient + ", " + SimulationTime + ")";
		}
	}
}
