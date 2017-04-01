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
		/// The speed limit, in miles per hour.
		/// </summary>
		public int SpeedLimit {
			get {
				return SpeedLimitImpl;
			}
			private set {
				SetProperty(ref SpeedLimitImpl, value);
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

		/// <summary>
		/// The length of the train, in feet.
		/// </summary>
		public int Length {
			get {
				return LengthImpl;
			}
			private set {
				SetProperty(ref LengthImpl, value);
			}
		}

		/// <summary>
		/// The weight of the train, in tons.
		/// </summary>
		public int Weight {
			get {
				return WeightImpl;
			}
			private set {
				SetProperty(ref WeightImpl, value);
			}
		}

		/// <summary>
		/// The horsepower per ton of the train.
		/// </summary>
		public float HPt {
			get {
				return HPtImpl;
			}
			private set {
				SetProperty(ref HPtImpl, value);
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

			// Timestamp the new arrival.
			DataLastUpdated = DateTime.UtcNow;

			// Update general properties.
			Company = data.RailroadInitials;
			LocoNumber = data.LocoNumber;
			Tag = data.TrainSymbol;
			Speed = (int) data.TrainSpeedMph;
			SpeedLimit = data.TrainSpeedLimitMPH;
			EngineerType = data.EngineerType;
			EngineerName = (EngineerType == EEngineerType.AI) ? "AI" : (EngineerType == EEngineerType.None) ? "No Driver" : data.EngineerName;
			Length = data.TrainLengthFeet;
			Weight = data.TrainWeightTons;
			HPt = data.HpPerTon;

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

		#region Interlocking API
		/// <summary>
		/// Checks whether this train has expired and probably left the area, updating it accordingly if so.
		/// </summary>
		/// <returns><c>true</c> if this train has expired, or <c>false</c> if it is still current.</returns>
		public bool CheckExpired() {
			bool expired = (DateTime.UtcNow - DataLastUpdated).TotalSeconds >= MaxAge;
			if (expired) {
				LocationCurrent = false;
			}
			return expired;
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
		private int SpeedLimitImpl;
		private string SubAreaImpl;
		private string LocationImpl;
		private bool LocationCurrentImpl;
		private EEngineerType EngineerTypeImpl;
		private string EngineerNameImpl;
		private int LengthImpl;
		private int WeightImpl;
		private float HPtImpl;
		#endregion
	}
}
