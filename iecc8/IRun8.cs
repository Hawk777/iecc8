using System;
using System.ServiceModel;

namespace Iecc8 {
	/// <summary>
	/// The API exposed by Run8, providing functions that the dispatcher uses to manipulate the world.
	/// </summary>
	[ServiceContract(CallbackContract = typeof(IDispatcher), Name = "IWCFRun8", SessionMode = SessionMode.Required)]
	public interface IRun8 {
		/// <summary>
		/// Orders an AI driver to board a train and start driving.
		/// </summary>
		/// <param name="pMessage">Which train to drive.</param>
		[OperationContract(AsyncPattern = true, IsOneWay = true)]
		IAsyncResult BeginAIRecrewTrain(Messages.AIRecrewTrainMessage pMessage, AsyncCallback cb, object state);

		void EndAIRecrewTrain(IAsyncResult result);

		/// <summary>
		/// Chooses whether a signal is on or off and, if off, whether it has automatic working enabled.
		/// </summary>
		/// <param name="pMessage">Details of the change to make.</param>
		[OperationContract(AsyncPattern = true, IsOneWay = true)]
		IAsyncResult BeginChangeSignal(Messages.DispatcherSignalMessage pMessage, AsyncCallback cb, object state);

		void EndChangeSignal(IAsyncResult result);

		/// <summary>
		/// Notifies Run8 that a dispatcher is ready to work.
		/// </summary>
		[OperationContract(AsyncPattern = true, IsOneWay = true)]
		IAsyncResult BeginDispatcherConnected(AsyncCallback cb, object state);

		void EndDispatcherConnected(IAsyncResult result);

		/// <summary>
		/// Orders an AI to brake and hold its position or resume normal driving.
		/// </summary>
		/// <param name="pMessage">Details of the orders.</param>
		[OperationContract(AsyncPattern = true, IsOneWay = true)]
		IAsyncResult BeginHoldAITrain(Messages.HoldAITrainMessage pMessage, AsyncCallback cb, object state);

		void EndHoldAITrain(IAsyncResult result);

		/// <summary>
		/// Does nothing.
		/// </summary>
		[OperationContract(AsyncPattern = true, IsOneWay = true)]
		IAsyncResult BeginPing(AsyncCallback cb, object state);

		void EndPing(IAsyncResult result);

		/// <summary>
		/// Sends a text message over a radio channel.
		/// </summary>
		/// <param name="pMessage">Details of the message to send.</param>
		[OperationContract(AsyncPattern = true, IsOneWay = true)]
		IAsyncResult BeginRadioText(Messages.RadioTextMessage pMessage, AsyncCallback cb, object state);

		void EndRadioText(IAsyncResult result);

		/// <summary>
		/// Orders an AI driver to disembark from its train once stopped, or to remain in its seat.
		/// </summary>
		/// <param name="pMessage">Details of the orders.</param>
		[OperationContract(AsyncPattern = true, IsOneWay = true)]
		IAsyncResult BeginRelinquishAITrain(Messages.RelinquishAITrainMessage pMessage, AsyncCallback cb, object state);

		void EndRelinquishAITrain(IAsyncResult result);

		/// <summary>
		/// Instantly stops all movement of an AI train orders the driver to hold position.
		/// </summary>
		/// <param name="pMessage">Which train to stop.</param>
		[OperationContract(AsyncPattern = true, IsOneWay = true)]
		IAsyncResult BeginStopAITrain(Messages.StopAITrainMessage pMessage, AsyncCallback cb, object state);

		void EndStopAITrain(IAsyncResult result);

		/// <summary>
		/// Drives power points to a position, unlocks them for hand cranking, or relocks them for power operation.
		/// </summary>
		/// <param name="pMessage">Details of the change to make.</param>
		[OperationContract(AsyncPattern = true, IsOneWay = true)]
		IAsyncResult BeginThrowSwitch(Messages.DispatcherSwitchMessage pMessage, AsyncCallback cb, object state);

		void EndThrowSwitch(IAsyncResult result);

		/// <summary>
		/// Teleports the player to power points.
		/// </summary>
		/// <param name="pMessage">Which points to teleport to.</param>
		[OperationContract(AsyncPattern = true, IsOneWay = true)]
		IAsyncResult BeginTransportPlayer(Messages.TransportPlayerMessage pMessage, AsyncCallback cb, object state);

		void EndTransportPlayer(IAsyncResult result);

		/// <summary>
		/// Teleports the player to a track circuit.
		/// </summary>
		/// <param name="pMessage">Which track circuit to teleport to.</param>
		[OperationContract(AsyncPattern = true, IsOneWay = true)]
		IAsyncResult BeginTransportPlayerToBlock(Messages.TransportPlayerToBlockMessage pMessage, AsyncCallback cb, object state);

		void EndTransportPlayerToBlock(IAsyncResult result);
	}
}
