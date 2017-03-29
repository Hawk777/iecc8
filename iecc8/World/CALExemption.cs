using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iecc8.World {
	/// <summary>
	/// An exemption from approach locking granted by comprehensive approach locking rules.
	/// </summary>
	public struct CALExemption {
		#region Common API
		/// <summary>
		/// The track circuits that must be clear for this exemption to apply.
		/// </summary>
		public readonly IReadOnlyList<TrackCircuit> ClearTCs;

		/// <summary>
		/// The points that must be in particular positions for this exemption to apply.
		/// </summary>
		public readonly IReadOnlyList<RoutePointPosition> PointPositions;

		/// <summary>
		/// The signals that must all be exhibiting either stop or restricting indications in order for this exemption to apply.
		/// </summary>
		public readonly IReadOnlyList<Signal> SignalsOn;

		/// <summary>
		/// Checks whether this exemption applies.
		/// </summary>
		/// <returns><c>true</c> if this exemption applies, or <c>false</c> if not.</returns>
		public bool Check() {
			foreach (TrackCircuit i in ClearTCs) {
				if (i.Occupied || i.ReversedHandPoints) {
					return false;
				}
			}
			foreach (RoutePointPosition i in PointPositions) {
				if ((i.Points.Reversed != i.Reverse) || !i.Points.Proved) {
					return false;
				}
			}
			foreach (Signal i in SignalsOn) {
				if (i.Aspects.Off) {
					ControlledSignal cs = i as ControlledSignal;
					if ((cs == null) || !cs.ApproachLocked) {
						return false;
					}
				}
			}
			return true;
		}
		#endregion

		#region Data Initialization API
		/// <summary>
		/// Constructs a new CALExemption.
		/// </summary>
		/// <param name="schema">The schema object containing data for this CAL exemption.</param>
		/// <param name="region">The region.</param>
		/// <param name="subArea">The ID number of the sub-area against which to resolve short object IDs.</param>
		public CALExemption(Schema.CALExemption schema, Region region, ushort subArea) {
			{
				TrackCircuit[] tcs = new TrackCircuit[schema.ClearTCs.Count];
				for (int i = 0; i != schema.ClearTCs.Count; ++i) {
					tcs[i] = region.GetTrackCircuit(schema.ClearTCs[i], subArea);
				}
				ClearTCs = Array.AsReadOnly(tcs);
			}
			{
				RoutePointPosition[] pointPositions = new RoutePointPosition[schema.PointPositions.Count];
				for (int i = 0; i != schema.PointPositions.Count; ++i) {
					pointPositions[i] = new RoutePointPosition(schema.PointPositions[i], region, subArea);
				}
				PointPositions = Array.AsReadOnly(pointPositions);
			}
			{
				Signal[] sigs = new Signal[schema.SignalsOn.Count];
				for (int i = 0; i != schema.SignalsOn.Count; ++i) {
					sigs[i] = region.GetSignal(schema.SignalsOn[i], subArea);
				}
				SignalsOn = Array.AsReadOnly(sigs);
			}
		}
		#endregion
	}
}
