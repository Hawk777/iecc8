namespace Iecc8.UI {
	/// <summary>
	/// A message which shows up in the message list.
	/// </summary>
	public struct Message {
		/// <summary>
		/// The types of messages.
		/// </summary>
		public enum EType {
			/// <summary>
			/// A radio text message.
			/// </summary>
			Radio,

			/// <summary>
			/// A miscellaneous system message.
			/// </summary>
			Miscellaneous,
		}

		/// <summary>
		/// The type of message.
		/// </summary>
		public EType Type {
			get;
			set;
		}

		/// <summary>
		/// The text of the message.
		/// </summary>
		public string Text {
			get;
			set;
		}
	}
}
