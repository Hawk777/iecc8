using Iecc8.Messages;
using Iecc8.Schema;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace Iecc8.World {
	/// <summary>
	/// A region, which may contain multiple sub-areas.
	/// </summary>
	public class Region {
		#region Common API
		/// <summary>
		/// The sub-areas in this region, keyed by Run8 ID number.
		/// </summary>
		public readonly IReadOnlyDictionary<ushort, SubArea> SubAreas;

		/// <summary>
		/// Finds a set of power points.
		/// </summary>
		/// <param name="id">The ID of the points, which can be long or short.</param>
		/// <param name="subAreaID">The ID of the sub-area containing the object doing the lookup, which will be used to resolve a short points ID.</param>
		/// <returns>The points.</returns>
		public Points GetPowerPoints(uint id, ushort subAreaID) {
			if (id >= 1000) {
				Debug.Assert(id < 1000000);
				subAreaID = (ushort) (id / 1000);
				id %= 1000;
			}
			return SubAreas[subAreaID].PowerPoints[(ushort) id];
		}

		/// <summary>
		/// Finds a signal.
		/// </summary>
		/// <param name="id">The ID of the signal, which can be long or short.</param>
		/// <param name="subAreaID">The ID of the sub-area containing the object doing the lookup, which will be used to resolve a short signal ID.</param>
		/// <returns>The signal.</returns>
		public Signal GetSignal(int id, ushort subAreaID) {
			if (id >= 1000) {
				Debug.Assert(id < 1000000);
				subAreaID = (ushort) (id / 1000);
				id %= 1000;
			} else if (id <= -1000) {
				Debug.Assert(id > -1000000);
				subAreaID = (ushort) (-id / 1000);
				id %= 1000;
			}
			return SubAreas[subAreaID].Signals[(short) id];
		}

		/// <summary>
		/// Finds a track circuit.
		/// </summary>
		/// <param name="id">The ID of the track circuit, which can be long or short.</param>
		/// <param name="subAreaID">The ID of the sub-area containing the object doing the lookup, which will be used to resolve a short TC ID.</param>
		/// <returns>The track circuit.</returns>
		public TrackCircuit GetTrackCircuit(uint id, ushort subAreaID) {
			if (id >= 1000) {
				Debug.Assert(id < 1000000);
				subAreaID = (ushort) (id / 1000);
				id %= 1000;
			}
			return SubAreas[subAreaID].TrackCircuits[(ushort) id];
		}
		#endregion

		#region Data Initialization API
		/// <summary>
		/// Constructs the region.
		/// </summary>
		/// <param name="data">The data to use to build the region.</param>
		/// <param name="world">The world in which this region will exist.</param>
		public Region(Schema.Region data, World world) {
			SortedList<ushort, Schema.SubArea> subData = new SortedList<ushort, Schema.SubArea>();
			foreach (RegionEntry i in data.SubAreas) {
				subData[i.ID] = (Schema.SubArea) Application.LoadComponent(new Uri("/iecc8;component/Region/" + data.Name + "/" + i.Name + ".xaml", UriKind.Relative));
			}
			SortedList<ushort, SubArea> subs = new SortedList<ushort, SubArea>();
			foreach (RegionEntry i in data.SubAreas) {
				subs[i.ID] = new SubArea(subData[i.ID], i.ID, world);
			}
			SubAreas = new ReadOnlyDictionary<ushort, SubArea>(subs);
			foreach (KeyValuePair<ushort, SubArea> i in subs) {
				i.Value.InitLinks(subData[i.Key], this);
			}
		}
		#endregion

		#region Run8 API
		/// <summary>
		/// Updates the occupancy of all track circuits in a sub-area in this region.
		/// </summary>
		/// <param name="message">The received data.</param>
		public void UpdateFromRun8(OccupiedBlocksMessage message) {
			SubArea subArea;
			if (SubAreas.TryGetValue((ushort) message.Route, out subArea)) {
				subArea.UpdateFromRun8(message);
			} else if (squelchUnknownSubAreas.Add(message.Route)) {
				Debug.Print("Received update for unknown sub-area " + message.Route + ".");
			}
		}

		/// <summary>
		/// Updates the inconsistent state of all power points in a sub-area in this region.
		/// </summary>
		/// <param name="message">The received data.</param>
		public void UpdateFromRun8(InterlockErrorSwitchesMessage message) {
			SubArea subArea;
			if (SubAreas.TryGetValue((ushort) message.Route, out subArea)) {
				subArea.UpdateFromRun8(message);
			} else if (squelchUnknownSubAreas.Add(message.Route)) {
				Debug.Print("Received update for unknown sub-area " + message.Route + ".");
			}
		}

		/// <summary>
		/// Updates the occupied state of all power points in a sub-area in this region.
		/// </summary>
		/// <param name="message">The received data.</param>
		public void UpdateFromRun8(OccupiedSwitchesMessage message) {
			SubArea subArea;
			if (SubAreas.TryGetValue((ushort) message.Route, out subArea)) {
				subArea.UpdateFromRun8(message);
			} else if(squelchUnknownSubAreas.Add(message.Route)) {
				Debug.Print("Received update for unknown sub-area " + message.Route + ".");
			}
		}

		/// <summary>
		/// Updates the positions of all power points in a sub-area in this region.
		/// </summary>
		/// <param name="message">The received data.</param>
		public void UpdateFromRun8(ReversedSwitchesMessage message) {
			SubArea subArea;
			if (SubAreas.TryGetValue((ushort) message.Route, out subArea)) {
				subArea.UpdateFromRun8(message);
			} else if (squelchUnknownSubAreas.Add(message.Route)) {
				Debug.Print("Received update for unknown sub-area " + message.Route + ".");
			}
		}

		/// <summary>
		/// Updates the hand-crankable status of all power points in a sub-area in this region.
		/// </summary>
		/// <param name="message">The received data.</param>
		public void UpdateFromRun8(UnlockedSwitchesMessage message) {
			SubArea subArea;
			if (SubAreas.TryGetValue((ushort) message.Route, out subArea)) {
				subArea.UpdateFromRun8(message);
			} else if (squelchUnknownSubAreas.Add(message.Route)) {
				Debug.Print("Received update for unknown sub-area " + message.Route + ".");
			}
		}

		/// <summary>
		/// Updates the indications of all controlled signals in this region.
		/// </summary>
		/// <param name="message">The received data.</param>
		public async Task UpdateFromRun8Async(SignalsMessage message) {
			SubArea subArea;
			if (SubAreas.TryGetValue((ushort) message.Route, out subArea)) {
				await subArea.UpdateFromRun8Async(message);
			} else if (squelchUnknownSubAreas.Add(message.Route)) {
				Debug.Print("Received update for unknown sub-area " + message.Route + ".");
			}
		}
		#endregion

		#region Private Members
		private HashSet<int> squelchUnknownSubAreas = new HashSet<int>();
		#endregion
	}
}
