using System.Diagnostics;
using System.Windows.Data;

namespace Iecc8.UI.TopLevel {
	/// <summary>
	/// A boolean mask of which radio channels are enabled for receiving.
	/// </summary>
	public class ChannelMask : BindableBase {
		/// <summary>
		/// Whether or not a particular channel is enabled.
		/// </summary>
		/// <returns><c>true</c> if the channel is enabled, or <c>false</c> if not.</returns>
		public bool this[int index] {
			get {
				return Impl[index];
			}
			set {
				Debug.Assert(value || (index != 0));
				SetProperty(ref Impl[index], value, Binding.IndexerName);
			}
		}

		/// <summary>
		/// Constructs a new ChannelMask.
		/// </summary>
		public ChannelMask() {
			Impl = new bool[100];
			Impl[0] = true;
		}

		private readonly bool[] Impl;
	}
}
