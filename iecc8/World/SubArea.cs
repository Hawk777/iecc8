using Iecc8.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Iecc8.World {
	/// <summary>
	/// A geographical sub-area.
	/// </summary>
	public class SubArea {
		#region Common API
		/// <summary>
		/// The sub-area ID.
		/// </summary>
		public readonly ushort ID;

		/// <summary>
		/// The name of this sub-area.
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// The track circuits in this sub-area.
		/// </summary>
		/// <remarks>
		/// The track circuits are indexed by their Run8 numerical IDs.
		/// </remarks>
		public readonly IReadOnlyList<TrackCircuit> TrackCircuits;

		/// <summary>
		/// The controlled signals in this sub-area.
		/// </summary>
		/// <remarks>
		/// The signals are indexed by their positions in the Run8 signal array.
		/// </remarks>
		public readonly IReadOnlyList<ControlledSignal> ControlledSignals;

		/// <summary>
		/// The automatic signals in this sub-area.
		/// </summary>
		public readonly IReadOnlyList<AutomaticSignal> AutomaticSignals;

		/// <summary>
		/// All the signals in this sub-area.
		/// </summary>
		public readonly SignalsArray Signals;

		/// <summary>
		/// The power points int his sub-area.
		/// </summary>
		/// <remarks>
		/// The points are indexed by their Run8 numerical IDs.
		/// </remarks>
		public readonly IReadOnlyList<Points> PowerPoints;
		#endregion

		#region Data Initialization API
		/// <summary>
		/// Constructs a new sub-area.
		/// </summary>
		/// <param name="schema">Data about this sub-area.</param>
		/// <param name="id">The sub-area ID.</param>
		/// <param name="world">The communication interface to Run8.</param>
		public SubArea(Schema.SubArea schema, ushort id, World world) {
			ID = id;
			Name = schema.Name ?? string.Empty;

			{
				TrackCircuit[] trackCircuits = new TrackCircuit[schema.TrackCircuits.Count];
				for (ushort i = 0; i != schema.TrackCircuits.Count; ++i) {
					trackCircuits[i] = new TrackCircuit(schema.TrackCircuits[i], ID, i);
				}
				TrackCircuits = Array.AsReadOnly(trackCircuits);
			}

			{
				Points[] points = new Points[schema.Points.Count];
				for (ushort i = 0; i != schema.Points.Count; ++i) {
					points[i] = new Points(schema.Points[i], i, this, world);
				}
				PowerPoints = Array.AsReadOnly(points);
			}

			{
				ControlledSignal[] sigs = new ControlledSignal[schema.ControlledSignals.Count];
				for (ushort i = 0; i != schema.ControlledSignals.Count; ++i) {
					sigs[i] = new ControlledSignal(schema.ControlledSignals[i], i, ID, world);
				}
				ControlledSignals = Array.AsReadOnly(sigs);
			}

			{
				AutomaticSignal[] sigs = new AutomaticSignal[schema.AutomaticSignals.Count];
				for (ushort i = 0; i != schema.AutomaticSignals.Count; ++i) {
					sigs[i] = new AutomaticSignal(schema.AutomaticSignals[i], (short) (-i - 1), ID);
				}
				AutomaticSignals = Array.AsReadOnly(sigs);
			}

			Signals = new SignalsArray(this);
		}

		/// <summary>
		/// Initializes the links between objects in this sub-area.
		/// </summary>
		/// <param name="schema">Data about this sub-area.</param>
		/// <param name="region">The region containing this sub-area.</param>
		public void InitLinks(Schema.SubArea schema, Region region) {
			for (ushort i = 0; i != schema.AutomaticSignals.Count; ++i) {
				AutomaticSignals[i].InitLinks(schema.AutomaticSignals[i], region);
			}

			for (ushort i = 0; i != schema.ControlledSignals.Count; ++i) {
				ControlledSignals[i].InitLinks(schema.ControlledSignals[i], region);
			}
		}
		#endregion

		#region Run8 API
		/// <summary>
		/// Updates the occupancy of all track circuits in this sub-area.
		/// </summary>
		/// <param name="message">The received data.</param>
		public void UpdateFromRun8(OccupiedBlocksMessage message) {
			message.OccupiedBlocks.Sort();
			message.OpenManualSwitchBlocks.Sort();
			IReadOnlyList<TrackCircuit> tcs = TrackCircuits;
			IEnumerator<int> occupiedEnum = message.OccupiedBlocks.GetEnumerator(), reversedEnum = message.OpenManualSwitchBlocks.GetEnumerator();
			occupiedEnum = occupiedEnum.MoveNext() ? occupiedEnum : null;
			reversedEnum = reversedEnum.MoveNext() ? reversedEnum : null;
			for (int i = 0; i != tcs.Count; ++i) {
				bool occupied = occupiedEnum != null && occupiedEnum.Current == i;
				bool reversed = reversedEnum != null && reversedEnum.Current == i;
				tcs[i].UpdateFromRun8(occupied, reversed);
				if (occupied) {
					occupiedEnum = occupiedEnum.MoveNext() ? occupiedEnum : null;
				}
				if (reversed) {
					reversedEnum = reversedEnum.MoveNext() ? reversedEnum : null;
				}
			}
			if (occupiedEnum != null) {
				Debug.Print("Run8 sent occupied track circuit number " + occupiedEnum.Current + " which sub-area " + ID + " doesn't know about.");
			}
			if (reversedEnum != null) {
				Debug.Print("Run8 sent track circuit with reversed hand points number " + reversedEnum.Current + " which sub-area " + ID + " doesn't know about.");
			}
		}

		/// <summary>
		/// Updates the inconsistent state of all power points in this sub-area.
		/// </summary>
		/// <param name="message">The received data.</param>
		public void UpdateFromRun8(InterlockErrorSwitchesMessage message) {
			message.InterlockErrorSwitches.Sort();
			IReadOnlyList<Points> pts = PowerPoints;
			ForEachNumberPresentAbsent((int i, bool error) => {
				pts[i].UpdateInterlockErrorFromRun8(error);
			}, pts.Count, message.InterlockErrorSwitches);
			if (message.InterlockErrorSwitches.Count > 0 && message.InterlockErrorSwitches[message.InterlockErrorSwitches.Count - 1] >= pts.Count && !PointsCountMismatchPrinted) {
				Debug.Print("Run8 sent inconsistent power points number " + message.InterlockErrorSwitches[message.InterlockErrorSwitches.Count - 1] + " which sub-area " + ID + " doesn't know about.");
				PointsCountMismatchPrinted = true;
			}
		}

		/// <summary>
		/// Updates the occupied state of all power points in this sub-area.
		/// </summary>
		/// <param name="message">The received data.</param>
		public void UpdateFromRun8(OccupiedSwitchesMessage message) {
			message.OccupiedSwitches.Sort();
			IReadOnlyList<Points> pts = PowerPoints;
			ForEachNumberPresentAbsent((int i, bool occupied) => {
				pts[i].UpdateOccupiedFromRun8(occupied);
			}, pts.Count, message.OccupiedSwitches);
			if (message.OccupiedSwitches.Count > 0 && message.OccupiedSwitches[message.OccupiedSwitches.Count - 1] >= pts.Count && !PointsCountMismatchPrinted) {
				Debug.Print("Run8 sent inconsistent power points number " + message.OccupiedSwitches[message.OccupiedSwitches.Count - 1] + " which sub-area " + ID + " doesn't know about.");
				PointsCountMismatchPrinted = true;
			}
		}

		/// <summary>
		/// Updates the positions of all power points in this sub-area.
		/// </summary>
		/// <param name="message">The received data.</param>
		public void UpdateFromRun8(ReversedSwitchesMessage message) {
			message.ReversedSwitches.Sort();
			IReadOnlyList<Points> pts = PowerPoints;
			ForEachNumberPresentAbsent((int i, bool reversed) => {
				pts[i].UpdateReversedFromRun8(reversed);
			}, pts.Count, message.ReversedSwitches);
			if (message.ReversedSwitches.Count > 0 && message.ReversedSwitches[message.ReversedSwitches.Count - 1] >= pts.Count && !PointsCountMismatchPrinted) {
				Debug.Print("Run8 sent reversed power points number " + message.ReversedSwitches[message.ReversedSwitches.Count - 1] + " which sub-area " + ID + " doesn't know about.");
				PointsCountMismatchPrinted = true;
			}
		}

		/// <summary>
		/// Updates the hand-crankable status of all power points in this sub-area.
		/// </summary>
		/// <param name="message">The received data.</param>
		public void UpdateFromRun8(UnlockedSwitchesMessage message) {
			message.UnlockedSwitches.Sort();
			IReadOnlyList<Points> pts = PowerPoints;
			ForEachNumberPresentAbsent((int i, bool unlocked) => {
				pts[i].UpdateUnlockedFromRun8(unlocked);
			}, pts.Count, message.UnlockedSwitches);
			if (message.UnlockedSwitches.Count > 0 && message.UnlockedSwitches[message.UnlockedSwitches.Count - 1] >= pts.Count && !PointsCountMismatchPrinted) {
				Debug.Print("Run8 sent hand-crankable power points number " + message.UnlockedSwitches[message.UnlockedSwitches.Count - 1] + " which sub-area " + ID + " doesn't know about.");
				PointsCountMismatchPrinted = true;
			}
		}

		/// <summary>
		/// Updates the indications of all controlled signals in this sub-area.
		/// </summary>
		/// <param name="message">The received data.</param>
		public async Task UpdateFromRun8Async(SignalsMessage message) {
			int count = Math.Min(ControlledSignals.Count, message.Signals.Count);
			Task[] tasks = new Task[count];
			for (int i = 0; i != count; ++i) {
				tasks[i] = ControlledSignals[i].UpdateFromRun8Async(message.Signals[i]);
			}
			foreach (Task i in tasks) {
				await i;
			}
			if (ControlledSignals.Count != message.Signals.Count && !SignalCountMismatchPrinted) {
				Debug.Print("Sub-area " + ID + " was configured with " + ControlledSignals.Count + " controlled signals but Run8 claims it has " + message.Signals.Count + ".");
				SignalCountMismatchPrinted = true;
			}
		}
		#endregion

		#region Private Members
		/// <summary>
		/// Whether we have printed a message regarding the number of controlled signals being wrong.
		/// </summary>
		private bool SignalCountMismatchPrinted = false;

		/// <summary>
		/// Whether we have printed a message regarding the number of power points being wrong.
		/// </summary>
		private bool PointsCountMismatchPrinted = false;

		/// <summary>
		/// The type of callback passed to ForEachNumberPresentAbsent.
		/// </summary>
		/// <param name="i">The index of the element being considered.</param>
		/// <param name="present"><c>true</c> if the element is present, or <c>false</c> if not.</param>
		private delegate void ForEachNumberPresentAbsentCB(int i, bool present);

		/// <summary>
		/// Invokes a callback once for each of a range of integers, passing each invocation a parameter indicating whether the current integer is present in a sorted collection.
		/// </summary>
		/// <param name="callback">The callback to invoke.</param>
		/// <param name="count">The number of integers to iterate over.</param>
		/// <param name="present">The collection of integers which are considered present, which must be sorted in ascending order.</param>
		private static void ForEachNumberPresentAbsent(ForEachNumberPresentAbsentCB callback, int count, IEnumerable<int> present) {
			IEnumerator<int> enm = present.GetEnumerator();
			enm = enm.MoveNext() ? enm : null;
			for (int i = 0; i != count; ++i) {
				bool currentPresent = enm != null && enm.Current == i;
				callback(i, currentPresent);
				if (currentPresent) {
					enm = enm.MoveNext() ? enm : null;
				}
			}
		}
		#endregion
	}
}
