using Iecc8.Schema;
using System.Diagnostics;

namespace Iecc8.World {
	/// <summary>
	/// A signal.
	/// </summary>
	public abstract class Signal : BindableBase {
		#region Common API
		/// <summary>
		/// Which sub-area contains this signal.
		/// </summary>
		public readonly ushort SubArea;

		/// <summary>
		/// This signal's internal Run8 ID number.
		/// </summary>
		public readonly short ID;

		/// <summary>
		/// The number of heads (lamps) in this signal.
		/// </summary>
		public readonly byte Heads;

		/// <summary>
		/// How far ahead this signal looks for diverging routes.
		/// </summary>
		public readonly EDivergenceLookahead DivergenceLookahead;

		/// <summary>
		/// The name of the signal.
		/// </summary>
		public string Name {
			get {
				return SubArea + "/" + ID;
			}
		}

		/// <summary>
		/// The aspects of the signal's heads.
		/// </summary>
		public Aspects Aspects {
			get {
				return AspectsImpl;
			}
			protected set {
				SetProperty(ref AspectsImpl, value);
			}
		}
		#endregion

		#region Data Initialization API
		/// <summary>
		/// Constructs a new Signal.
		/// </summary>
		/// <param name="subArea">Which sub-area contains this signal.</param>
		/// <param name="id">The ID number of the signal.</param>
		/// <param name="heads">The number of heads (lamps) in this signal.</param>
		protected Signal(ushort subArea, short id, byte heads, EDivergenceLookahead divergenceLookahead) {
			Debug.Assert(subArea >= 0);
			SubArea = subArea;
			ID = id;
			Heads = heads;
			DivergenceLookahead = divergenceLookahead;
		}
		#endregion

		#region Private Members
		private Aspects AspectsImpl;
		#endregion
	}
}
