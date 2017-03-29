using System.ComponentModel;
using System.Diagnostics;

namespace Iecc8.World {
	public class AutomaticSignal : Signal {
		#region Data Initialization API
		/// <summary>
		/// Constructs a new automatic signal.
		/// </summary>
		/// <param name="schema">The schema object containing data about this signal.</param>
		/// <param name="id">The position of this signal in the automatic signals array.</param>
		/// <param name="subArea">The ID of the sub-area that contains this signal.</param>
		public AutomaticSignal(Schema.AutomaticSignal schema, short id, ushort subArea) : base(subArea, id, schema.HeadCount, schema.DivergenceLookahead) {
			Debug.Assert(id < 0);
		}

		/// <summary>
		/// Connects this signal to other lineside equipment.
		/// </summary>
		/// <remarks>
		/// This must only be called by the SubArea constructor.
		/// </remarks>
		/// <param name="schema">The schema object containing data about this signal.</param>
		/// <param name="region">The region that contains this signal.</param>
		public void InitLinks(Schema.AutomaticSignal schema, Region region) {
			if (schema.ProtectedTC.HasValue) {
				ProtectedTrackCircuit = region.GetTrackCircuit(schema.ProtectedTC.Value, SubArea);
				ProtectedTrackCircuit.PropertyChanged += OnTCPropChanged;
			}
			if (schema.NextSignal.HasValue) {
				NextSignal = region.GetSignal(schema.NextSignal.Value, SubArea);
				NextSignal.PropertyChanged += OnNextSignalPropChanged;
			}
			UpdateAspects();
		}
		#endregion

		#region Private Members
		/// <summary>
		/// Which track circuit this signal protects, or <c>null</c> for special-purpose automatic signals that do not protect a TC such as those at the end of the universe.
		/// </summary>
		private TrackCircuit ProtectedTrackCircuit;

		/// <summary>
		/// The next signal in the direction of travel, or <c>null</c> for special-purpose automatic signals that do not have a next signal such as those at the end of the universe.
		/// </summary>
		private Signal NextSignal;

		/// <summary>
		/// Invoked when a property on the next signal changes.
		/// </summary>
		/// <param name="sender">The next signal.</param>
		/// <param name="e">Details about the change.</param>
		private void OnNextSignalPropChanged(object sender, PropertyChangedEventArgs e) {
			UpdateAspects();
		}

		/// <summary>
		/// Invoked when a property on the protected track circuit changes.
		/// </summary>
		/// <param name="sender">The protected track circuit.</param>
		/// <param name="e">Details about the change.</param>
		private void OnTCPropChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(TrackCircuit.Occupied) || e.PropertyName == nameof(TrackCircuit.ReversedHandPoints)) {
				UpdateAspects();
			}
		}

		/// <summary>
		/// Calculates and sets new signal aspects.
		/// </summary>
		private void UpdateAspects() {
			if ((ProtectedTrackCircuit != null) && (ProtectedTrackCircuit.Occupied || ProtectedTrackCircuit.ReversedHandPoints)) {
				Aspects = new Aspects(EAspectsType.Red, 0);
			} else if (NextSignal != null) {
				Aspects = NextSignal.Aspects.NextInRear(DivergenceLookahead);
			} else {
				Aspects = new Aspects(EAspectsType.Red, 0);
			}
		}
		#endregion
	}
}
