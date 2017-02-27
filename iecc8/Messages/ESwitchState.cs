using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// The possible actions that can be taken on power points.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromDispatcher")]
	public enum ESwitchState {
		/// <summary>
		/// Moves the points to their normal position.
		/// </summary>
		/// <remarks>
		/// The points must be locked. Unlocked points ignore this action.
		/// </remarks>
		[EnumMember]
		Normal,

		/// <summary>
		/// Moves the points to their reverse position.
		/// </summary>
		/// <remarks>
		/// The points must be locked. Unlocked points ignore this action.
		/// </remarks>
		[EnumMember]
		Reversed,

		/// <summary>
		/// Locks the points so they can be power-operated.
		/// </summary>
		/// <remarks>
		/// The points must be unlocked and in a consistent position. Locked points and those in inconsistent positions ignore this action.
		/// </remarks>
		[EnumMember]
		Locked,

		/// <summary>
		/// Unlocks points so they can be hand-cranked.
		/// </summary>
		[EnumMember]
		Unlocked,
	}
}
