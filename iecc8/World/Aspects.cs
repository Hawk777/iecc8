using Iecc8.Schema;
using System.Diagnostics;

namespace Iecc8.World {
	/// <summary>
	/// The full collection of information about what aspects a signal is exhibiting.
	/// </summary>
	public struct Aspects {
		/// <summary>
		/// What type of aspect combination this is.
		/// </summary>
		public EAspectsType Type;

		/// <summary>
		/// Which signal head is identified for type-specific special treatment.
		/// </summary>
		public byte IdentifiedHead;

		/// <summary>
		/// Whether this signal is on (showing a stop indication).
		/// </summary>
		/// <remarks>
		/// Restricting indications are considered on, as this is more useful for most purposes.
		/// </remarks>
		public bool On {
			get {
				return (Type == EAspectsType.Red) || (Type == EAspectsType.AllLunar) || (Type == EAspectsType.OneLunar);
			}
		}

		/// <summary>
		/// Whether this signal is off (showing a proceed indication).
		/// </summary>
		public bool Off {
			get {
				return !On;
			}
		}

		/// <summary>
		/// Constructs a new Aspects.
		/// </summary>
		/// <param name="type">The type of aspect collection.</param>
		/// <param name="identifiedHead">The identified signal head.</param>
		public Aspects(EAspectsType type, byte identifiedHead) {
			switch (type) {
				case EAspectsType.Red:
				case EAspectsType.AllLunar:
				case EAspectsType.Yellow:
				case EAspectsType.FlashingYellow:
				case EAspectsType.Green:
					// These do not use the identified head for anything. Coalesce for ease of comparison.
					identifiedHead = 0;
					break;

				case EAspectsType.OneLunar:
					// Here the identified head can be anything.
					break;

				case EAspectsType.RedOverYellow:
				case EAspectsType.RedOverFlashingYellow:
				case EAspectsType.RedOverGreen:
				case EAspectsType.YellowOverYellow:
				case EAspectsType.YellowOverGreen:
					// Here the identified head must not be the top head.
					Debug.Assert(identifiedHead != 0);
					break;
			}
			Type = type;
			IdentifiedHead = identifiedHead;
		}

		/// <summary>
		/// Returns what aspect is present at a particular head.
		/// </summary>
		/// <param name="head">The head number.</param>
		/// <returns>The aspect.</returns>
		public EAspect AspectAt(byte head) {
			switch (Type) {
				case EAspectsType.Red:
					return EAspect.Red;
				case EAspectsType.AllLunar:
					return EAspect.Lunar;
				case EAspectsType.OneLunar:
					return (head == IdentifiedHead) ? EAspect.Lunar : EAspect.Red;
				case EAspectsType.Yellow:
					return (head == 0) ? EAspect.Yellow : EAspect.Red;
				case EAspectsType.FlashingYellow:
					return (head == 0) ? EAspect.FlashingYellow : EAspect.Red;
				case EAspectsType.Green:
					return (head == 0) ? EAspect.Green : EAspect.Red;
				case EAspectsType.RedOverYellow:
					return (head == IdentifiedHead) ? EAspect.Yellow : EAspect.Red;
				case EAspectsType.RedOverFlashingYellow:
					return (head == IdentifiedHead) ? EAspect.FlashingYellow : EAspect.Red;
				case EAspectsType.RedOverGreen:
					return (head == IdentifiedHead) ? EAspect.Green : EAspect.Red;
				case EAspectsType.YellowOverYellow:
					return ((head == 0) || (head == IdentifiedHead)) ? EAspect.Yellow : EAspect.Red;
				case EAspectsType.YellowOverGreen:
					return (head == 0) ? EAspect.Yellow : (head == IdentifiedHead) ? EAspect.Green : EAspect.Red;
			}
			return EAspect.Red;
		}

		/// <summary>
		/// Computes the aspects object shown in rear of this one, assuming an unblocked straight route.
		/// </summary>
		/// <param name="divergenceLookahead">How far ahead to consider divergences.</param>
		/// <returns>The aspects in rear of this one.</returns>
		public Aspects NextInRear(EDivergenceLookahead divergenceLookahead) {
			switch (Type) {
				case EAspectsType.Red:
				case EAspectsType.AllLunar:
				case EAspectsType.OneLunar:
					return new Aspects(EAspectsType.Yellow, 0);

				case EAspectsType.Yellow:
					return new Aspects(EAspectsType.FlashingYellow, 0);

				case EAspectsType.FlashingYellow:
				case EAspectsType.Green:
					return new Aspects(EAspectsType.Green, 0);

				case EAspectsType.RedOverYellow:
					return new Aspects((divergenceLookahead != EDivergenceLookahead.Short) ? EAspectsType.YellowOverYellow : EAspectsType.FlashingYellow, IdentifiedHead);

				case EAspectsType.RedOverFlashingYellow:
					return new Aspects((divergenceLookahead != EDivergenceLookahead.Short) ? EAspectsType.YellowOverYellow : EAspectsType.Green, IdentifiedHead);

				case EAspectsType.RedOverGreen:
					return new Aspects((divergenceLookahead != EDivergenceLookahead.Short) ? EAspectsType.YellowOverGreen : EAspectsType.Green, IdentifiedHead);

				case EAspectsType.YellowOverYellow:
				case EAspectsType.YellowOverGreen:
					return new Aspects((divergenceLookahead == EDivergenceLookahead.Long) ? EAspectsType.FlashingYellow : EAspectsType.Green, 0);
			}
			return new Aspects(EAspectsType.Red, 0);
		}

		/// <summary>
		/// Computes the aspects object shown in rear of this one, assuming an unblocked diverging route.
		/// </summary>
		/// <param name="divergenceNumber">What number of diverging route is taken from this signal.</param>
		/// <returns>The aspects in rear of this one.</returns>
		public Aspects NextInRearDiverging(byte divergenceNumber) {
			switch (Type) {
				case EAspectsType.Red:
				case EAspectsType.AllLunar:
				case EAspectsType.OneLunar:
					return new Aspects(EAspectsType.RedOverYellow, divergenceNumber);

				case EAspectsType.Yellow:
				case EAspectsType.RedOverYellow:
					return new Aspects(EAspectsType.RedOverFlashingYellow, divergenceNumber);

				case EAspectsType.FlashingYellow:
				case EAspectsType.Green:
				case EAspectsType.RedOverFlashingYellow:
				case EAspectsType.RedOverGreen:
				case EAspectsType.YellowOverYellow:
				case EAspectsType.YellowOverGreen:
					return new Aspects(EAspectsType.RedOverGreen, divergenceNumber);
			}
			return new Aspects(EAspectsType.Red, 0);
		}

		/// <summary>
		/// Compares two aspect sets for equality.
		/// </summary>
		/// <param name="x">The first aspect set.</param>
		/// <param name="y">The second aspect set.</param>
		/// <returns><c>true</c> if <paramref name="x"/> and <paramref name="y"/> are equal, or <c>false</c> if not.</returns>
		public static bool operator ==(Aspects x, Aspects y) {
			return (x.Type == y.Type) && (x.IdentifiedHead == y.IdentifiedHead);
		}

		/// <summary>
		/// Compares two aspect sets for inequality.
		/// </summary>
		/// <param name="x">The first aspect set.</param>
		/// <param name="y">The second aspect set.</param>
		/// <returns><c>false</c> if <paramref name="x"/> and <paramref name="y"/> are equal, or <c>true</c> if not.</returns>
		public static bool operator !=(Aspects x, Aspects y) {
			return !(x == y);
		}

		/// <summary>
		/// Compares two aspect sets for inequality.
		/// </summary>
		/// <param name="other">The aspect set to compare to.</param>
		/// <returns><c>true</c> if <paramref name="other"/> is equal to this, or <c>false</c> if not.</returns>
		public bool Equals(Aspects other) {
			return this == other;
		}

		public override bool Equals(object obj) {
			return (obj is Aspects) && Equals((Aspects) obj);
		}

		public override int GetHashCode() {
			return (Type.GetHashCode() * 65537) ^ IdentifiedHead.GetHashCode();
		}
	}
}
