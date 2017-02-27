using System.Runtime.Serialization;
using System.Text;

namespace Iecc8.Messages {
	/// <summary>
	/// Information about a train.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromRun8")]
	public struct TrainData {
		/// <summary>
		/// The number of axles in the train's rolling stock.
		/// </summary>
		[DataMember(Name = "<AxleCount>k__BackingField")]
		public int AxleCount;

		/// <summary>
		/// Which track circuit the train is currently in.
		/// </summary>
		[DataMember(Name = "<BlockID>k__BackingField")]
		public int BlockID;

		/// <summary>
		/// The name of the player driving the train, if any.
		/// </summary>
		/// <remarks>
		/// AI drivers, although given names inside Run8, do not populate this field.
		/// </remarks>
		[DataMember(Name = "<EngineerName>k__BackingField")]
		public string EngineerName;

		/// <summary>
		/// What type of driver is on board.
		/// </summary>
		[DataMember(Name = "<EngineerType>k__BackingField")]
		public EEngineerType EngineerType;

		/// <summary>
		/// Whether the AI driver on board this train has been ordered to brake and hold position.
		/// </summary>
		[DataMember(Name = "<HoldingForDispatcher>k__BackingField")]
		public bool HoldingForDispatcher;

		/// <summary>
		/// The number of horsepower of tractive power attached to the train divided by the number of tons of rolling stock.
		/// </summary>
		/// <remarks>
		/// In computing horsepower, only locomotives with engine isolation switches set to Run are included, regardless of their circuit breaker settings or whether their engines are actually running. In computing tons, only wagons and carriages, not locomotives, are included.
		/// </remarks>
		[DataMember(Name = "<HpPerTon>k__BackingField")]
		public float HpPerTon;

		/// <summary>
		/// The numerical part of the locomotive's identifier.
		/// </summary>
		/// <example>
		/// For locomotive BNSF1234, this field would be set to 1234.
		/// </example>
		[DataMember(Name = "<LocoNumber>k__BackingField")]
		public int LocoNumber;

		/// <summary>
		/// The string part of the locomotive's identifier.
		/// </summary>
		/// <example>
		/// For locomotive BNSF1234, this field would be set to BNSF.
		/// </example>
		[DataMember(Name = "<RailroadInitials>k__BackingField")]
		public string RailroadInitials;

		/// <summary>
		/// Whether the AI driver on board this train has been ordered to disembark once the train is stopped.
		/// </summary>
		[DataMember(Name = "<RelinquishWhenStopped>k__BackingField")]
		public bool RelinquishWhenStopped;

		/// <summary>
		/// The internal ID number used to refer to the train in external dispatcher protocol messages.
		/// </summary>
		[DataMember(Name = "<TrainID>k__BackingField")]
		public int TrainID;

		/// <summary>
		/// The length of the train in feet.
		/// </summary>
		[DataMember(Name = "<TrainLengthFeet>k__BackingField")]
		public int TrainLengthFeet;

		/// <summary>
		/// The train's speed limit, considering both rolling stock and occupied track.
		/// </summary>
		[DataMember(Name = "<TrainSpeedLimitMPH>k__BackingField")]
		public int TrainSpeedLimitMPH;

		/// <summary>
		/// The train's current speed.
		/// </summary>
		/// <remarks>
		/// This field is negative if the controlling locomotive is moving backwards.
		/// </remarks>
		[DataMember(Name = "<TrainSpeedMph>k__BackingField")]
		public float TrainSpeedMph;

		/// <summary>
		/// The train's symbol.
		/// </summary>
		/// <example>
		/// M-RICBAR
		/// </example>
		[DataMember(Name = "<TrainSymbol>k__BackingField")]
		public string TrainSymbol;

		/// <summary>
		/// The weight of the train's wagons or carriages, in tons.
		/// </summary>
		/// <remarks>
		/// Locomotives are not included in this number.
		/// </remarks>
		[DataMember(Name = "<TrainWeightTons>k__BackingField")]
		public int TrainWeightTons;

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append(nameof(TrainData));
			sb.Append('(');
			sb.Append(TrainID);
			sb.Append(", ");
			sb.Append(RailroadInitials);
			sb.Append(LocoNumber);
			sb.Append(", ");
			sb.Append(TrainSymbol);
			sb.Append(", ");
			sb.Append(EngineerName);
			sb.Append(" (");
			sb.Append(EngineerType);
			sb.Append("), ");
			sb.Append(TrainSpeedMph);
			sb.Append('/');
			sb.Append(TrainSpeedLimitMPH);
			sb.Append(" mph, ");
			sb.Append(TrainWeightTons);
			sb.Append(" t, ");
			sb.Append(AxleCount);
			sb.Append(" axles, ");
			sb.Append(HpPerTon);
			sb.Append(" HP/t, ");
			sb.Append(TrainLengthFeet);
			sb.Append("', block ");
			sb.Append(BlockID);
			if (HoldingForDispatcher) {
				sb.Append(", HoldingForDispatcher");
			}
			if (RelinquishWhenStopped) {
				sb.Append(", RelinquishWhenStopped");
			}
			return sb.ToString();
		}
	}
}
