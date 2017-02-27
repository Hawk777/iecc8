using System.ServiceModel;

namespace Iecc8 {
	/// <summary>
	/// The API exposed by the dispatcher, providing callbacks that Run8 invokes to notify the dispatcher of state changes in the world.
	/// </summary>
	public interface IDispatcher {
		/// <summary>
		/// Notifies of a radio tone received by a tower.
		/// </summary>
		/// <param name="pMessage">Details of the tone.</param>
		[OperationContract(IsOneWay = true)]
		void DTMF(Messages.DTMFMessage pMessage);

		/// <summary>
		/// Notifies of a change in the dispatcher client's access level.
		/// </summary>
		/// <param name="pMessage">Details of the access level change.</param>
		[OperationContract(IsOneWay = true)]
		void PermissionUpdate(Messages.DispatcherPermissionMessage pMessage);

		/// <summary>
		/// Does nothing.
		/// </summary>
		[OperationContract(IsOneWay = true)]
		void Ping();

		/// <summary>
		/// Notifies of a text message sent to a radio channel by a player.
		/// </summary>
		/// <param name="pMessage">Details of the radio message.</param>
		[OperationContract(IsOneWay = true)]
		void RadioText(Messages.RadioTextMessage pMessage);

		/// <summary>
		/// Notifies of the current in-game date and time and whether the game is connected to a multiplayer host.
		/// </summary>
		/// <param name="pMessage">Details of the new situation.</param>
		[OperationContract(IsOneWay = true)]
		void SendSimulationState(Messages.SimulationStateMessage pMessage);

		/// <summary>
		/// Notifies of which power points have been hand-cranked to an inconsistent position.
		/// </summary>
		/// <remarks>
		/// An example is a pair of points forming a crossover with one side cranked normal and the other cranked reverse.
		/// </remarks>
		/// <param name="pMessage">The list of inconsistent power points.</param>
		[OperationContract(IsOneWay = true)]
		void SetInterlockErrorSwitches(Messages.InterlockErrorSwitchesMessage pMessage);

		/// <summary>
		/// Notifies of which power points have trains over them.
		/// </summary>
		/// <param name="pMessage">The list of power points with trains over them.</param>
		[OperationContract(IsOneWay = true)]
		void SetOccupiedSwitches(Messages.OccupiedSwitchesMessage pMessage);

		/// <summary>
		/// Notifies of which track circuits have trains in them.
		/// </summary>
		/// <param name="pMessage">The list of occupied track circuits.</param>
		[OperationContract(IsOneWay = true)]
		void SetOccupiedBlocks(Messages.OccupiedBlocksMessage pMessage);

		/// <summary>
		/// Notifies of which power points are in the reverse position.
		/// </summary>
		/// <remarks>
		/// This includes both points which are driven to the reverse position under signaller control and also unlocked points which are hand-cranked reverse.
		/// </remarks>
		/// <param name="pMessage">The list of reversed power points.</param>
		[OperationContract(IsOneWay = true)]
		void SetReversedSwitches(Messages.ReversedSwitchesMessage pMessage);

		/// <summary>
		/// Notifies of the state of signals.
		/// </summary>
		/// <param name="pMessage">The states of the signals in the area.</param>
		[OperationContract(IsOneWay = true)]
		void SetSignals(Messages.SignalsMessage pMessage);

		/// <summary>
		/// Notifies of which power points are unlocked for hand cranking.
		/// </summary>
		/// <param name="pMessage">The list of unlocked power points.</param>
		[OperationContract(IsOneWay = true)]
		void SetUnlockedSwitches(Messages.UnlockedSwitchesMessage pMessage);

		/// <summary>
		/// Provides information about a train in the area.
		/// </summary>
		/// <param name="pMessage">Details about one train.</param>
		[OperationContract(IsOneWay = true)]
		void UpdateTrainData(Messages.TrainDataMessage pMessage);
	}
}
