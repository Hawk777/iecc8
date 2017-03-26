using Iecc8.Messages;
using System;
using System.Diagnostics;

namespace Iecc8.World {
	/// <summary>
	/// A train.
	/// </summary>
	public class Train : BindableBase {
		#region Common API
		/// <summary>
		/// The Run8-internal ID used to refer to the train.
		/// </summary>
		public readonly int ID;

		/// <summary>
		/// Whether this train has expired and probably left the region.
		/// </summary>
		public bool Expired {
			get {
				return (DateTime.UtcNow - DataLastUpdated).TotalSeconds > MaxAge;
			}
		}

		/// <summary>
		/// The name of the owning company of the locomotive.
		/// </summary>
		public string Company {
			get {
				return CompanyImpl;
			}
			private set {
				SetProperty(ref CompanyImpl, value);
			}
		}

		/// <summary>
		/// The locomotive number.
		/// </summary>
		public int LocoNumber {
			get {
				return LocoNumberImpl;
			}
			private set {
				SetProperty(ref LocoNumberImpl, value);
			}
		}

		/// <summary>
		/// The destination tag.
		/// </summary>
		public string Tag {
			get {
				return TagImpl;
			}
			private set {
				SetProperty(ref TagImpl, value);
			}
		}

		/// <summary>
		/// The speed, in miles per hour.
		/// </summary>
		public int Speed {
			get {
				return SpeedImpl;
			}
			private set {
				SetProperty(ref SpeedImpl, value);
			}
		}

		/// <summary>
		/// The last known sub-area.
		/// </summary>
		public string SubArea {
			get {
				return SubAreaImpl;
			}
			private set {
				SetProperty(ref SubAreaImpl, value);
			}
		}

		/// <summary>
		/// The last known location.
		/// </summary>
		public string Location {
			get {
				return LocationImpl;
			}
			private set {
				SetProperty(ref LocationImpl, value);
			}
		}

		/// <summary>
		/// Whether the Location property has the current location, as opposed to an old location.
		/// </summary>
		public bool LocationCurrent {
			get {
				return LocationCurrentImpl;
			}
			private set {
				SetProperty(ref LocationCurrentImpl, value);
			}
		}

		/// <summary>
		/// The type of engineer driving the train.
		/// </summary>
		public EEngineerType EngineerType {
			get {
				return EngineerTypeImpl;
			}
			private set {
				SetProperty(ref EngineerTypeImpl, value);
			}
		}

		/// <summary>
		/// The name of the engineer driving the train, or AI for an AI train.
		/// </summary>
		public string EngineerName {
			get {
				return EngineerNameImpl;
			}
			private set {
				SetProperty(ref EngineerNameImpl, value);
			}
		}
		#endregion

		#region Data Initialization API
		/// <summary>
		/// Constructs a new Train.
		/// </summary>
		/// <param name="data">The initial data to populate with.</param>
		/// <param name="world">The world the train lives in.</param>
		public Train(TrainData data, World world) {
			ID = data.TrainID;
			World = world;
			LocationImpl = string.Empty;
			UpdateFromRun8(data);
		}
		#endregion

		#region Run8 API
		/// <summary>
		/// Accepts a packet from Run8.
		/// </summary>
		/// <param name="data">The data packet.</param>
		public void UpdateFromRun8(TrainData data) {
			// Sanity check.
			Debug.Assert(data.TrainID == ID);

			// Timestamp the new arrival and update the expired status.
			bool oldExpired = Expired;
			DataLastUpdated = DateTime.UtcNow;
			if (oldExpired) {
				EmitPropertyChanged(nameof(Expired));
			}

			// Update general properties.
			Company = data.RailroadInitials;
			LocoNumber = data.LocoNumber;
			Tag = data.TrainSymbol;
			Speed = (int) data.TrainSpeedMph;
			EngineerType = data.EngineerType;
			EngineerName = (EngineerType == EEngineerType.AI) ? "AI" : data.EngineerName;

			// Update location, keeping the old string if not available.
			SubArea sub = null;
			TrackCircuit tc = null;
			if (data.BlockID >= 0) {
				ushort subAreaID, tcID;
				if (data.BlockID >= 100000) {
					subAreaID = (ushort) (data.BlockID / 1000);
					tcID = (ushort) (data.BlockID % 1000);
				} else if (data.BlockID >= 10000) {
					subAreaID = (ushort) (data.BlockID / 100);
					tcID = (ushort) (data.BlockID % 100);
				} else {
					subAreaID = (ushort) (data.BlockID / 10);
					tcID = (ushort) (data.BlockID % 10);
				}
				if (World.Region.SubAreas.TryGetValue(subAreaID, out sub)) {
					if (tcID < sub.TrackCircuits.Count) {
						tc = sub.TrackCircuits[tcID];
					}
				}
			}
			if (tc != null) {
				SubArea = sub.Name;
				Location = tc.LocationName;
			}
			LocationCurrent = tc != null;
		}
		#endregion

		#region Private Members
		/// <summary>
		/// How old, in seconds, the data can be before it is considered expired.
		/// </summary>
		private const uint MaxAge = 5;

		/// <summary>
		/// The world the train lives in.
		/// </summary>
		private readonly World World;

		/// <summary>
		/// When an update was most recently received for this train.
		/// </summary>
		private DateTime DataLastUpdated;

		private string CompanyImpl;
		private int LocoNumberImpl;
		private string TagImpl;
		private int SpeedImpl;
		private string SubAreaImpl;
		private string LocationImpl;
		private bool LocationCurrentImpl;
		private EEngineerType EngineerTypeImpl;
		private string EngineerNameImpl;
		#endregion
	}
}
