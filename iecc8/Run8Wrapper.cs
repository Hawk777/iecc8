using Iecc8.Messages;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Iecc8 {
	/// <summary>
	/// Provides a tidier, easier-to-use way to call functions on Run8. Parameters are separated rather than packed in aggregates and all methods are async instead of Begin/End.
	/// </summary>
	public class Run8Wrapper {
		/// <summary>
		/// Constructs a new Run8Wrapper.
		/// </summary>
		/// <param name="run8">The communication link to wrap.</param>
		public Run8Wrapper(IRun8 run8) {
			Debug.Assert(run8 != null);
			Run8 = run8;
		}

		/// <summary>
		/// Orders an AI driver to board a train and start driving.
		/// </summary>
		/// <param name="train">Which train to drive.</param>
		public Task AIRecrewTrainAsync(int train) {
			AIRecrewTrainMessage msg;
			msg.TrainID = train;
			return Task.Factory.FromAsync(Run8.BeginAIRecrewTrain, Run8.EndAIRecrewTrain, msg, null);
		}

		/// <summary>
		/// Chooses whether a signal is on or off and, if off, whether it has automatic working enabled.
		/// </summary>
		/// <param name="route">The route containing the signal.</param>
		/// <param name="signal">The ID number of the signal.</param>
		/// <param name="indication">Which indication to set.</param>
		public Task ChangeSignalAsync(int route, int signal, ESignalIndication indication) {
			DispatcherSignalMessage msg;
			msg.Route = route;
			msg.SignalID = signal;
			msg.SignalIndication = indication;
			return Task.Factory.FromAsync(Run8.BeginChangeSignal, Run8.EndChangeSignal, msg, null);
		}

		/// <summary>
		/// Notifies Run8 that a dispatcher is ready to work.
		/// </summary>
		public Task DispatcherConnectedAsync() {
			return Task.Factory.FromAsync(Run8.BeginDispatcherConnected, Run8.EndDispatcherConnected, null);
		}

		/// <summary>
		/// Orders an AI to brake and hold its position or resume normal driving.
		/// </summary>
		/// <param name="train">Which train to send orders to.</param>
		/// <param name="hold"><c>true</c> if the AI driver should brake the train and hold its position, or <c>false</c> if the AI driver should drive normally.</param>
		public Task HoldAITrainAsync(int train, bool hold) {
			HoldAITrainMessage msg;
			msg.TrainID = train;
			msg.Hold = hold;
			return Task.Factory.FromAsync(Run8.BeginHoldAITrain, Run8.EndHoldAITrain, msg, null);
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		public Task PingAsync() {
			return Task.Factory.FromAsync(Run8.BeginPing, Run8.EndPing, null);
		}

		/// <summary>
		/// Sends a text message over a radio channel.
		/// </summary>
		/// <param name="channel">Which channel the message was received on or will be sent on.</param>
		/// <param name="text">The message.</param>
		public Task RadioTextAsync(int channel, string text) {
			RadioTextMessage msg;
			msg.Channel = channel;
			msg.Text = text;
			return Task.Factory.FromAsync(Run8.BeginRadioText, Run8.EndRadioText, msg, null);
		}

		/// <summary>
		/// Orders an AI driver to disembark from its train once stopped, or to remain in its seat.
		/// </summary>
		/// <param name="train">Which train to send orders to.</param>
		/// <param name="relinquish"><c>true</c> if the AI driver should disembark the train once it has stopped, or <c>false</c> if the driver should remain on board.</param>
		public Task RelinquishAITrainAsync(int train, bool relinquish) {
			RelinquishAITrainMessage msg;
			msg.TrainID = train;
			msg.RelinquishAITrainWhenStopped = relinquish;
			return Task.Factory.FromAsync(Run8.BeginRelinquishAITrain, Run8.EndRelinquishAITrain, msg, null);
		}

		/// <summary>
		/// Instantly stops all movement of an AI train orders the driver to hold position.
		/// </summary>
		/// <param name="train">Which train to stop.</param>
		public Task StopAITrainAsync(int train) {
			StopAITrainMessage msg;
			msg.TrainID = train;
			return Task.Factory.FromAsync(Run8.BeginStopAITrain, Run8.EndStopAITrain, msg, null);
		}

		/// <summary>
		/// Drives power points to a position, unlocks them for hand cranking, or relocks them for power operation.
		/// </summary>
		/// <param name="route">Which sub-area the points are in.</param>
		/// <param name="points">Which points to modify.</param>
		/// <param name="state">What action the points should take.</param>
		public Task ThrowSwitchAsync(int route, int points, ESwitchState state) {
			DispatcherSwitchMessage msg;
			msg.Route = route;
			msg.SwitchID = points;
			msg.SwitchState = state;
			return Task.Factory.FromAsync(Run8.BeginThrowSwitch, Run8.EndThrowSwitch, msg, null);
		}

		/// <summary>
		/// Teleports the player to power points.
		/// </summary>
		/// <param name="route">The sub-area to teleport to.</param>
		/// <param name="points">The power points to teleport to.</param>
		public Task TransportPlayerAsync(int route, int points) {
			TransportPlayerMessage msg;
			msg.Route = route;
			msg.SwitchIndex = points;
			return Task.Factory.FromAsync(Run8.BeginTransportPlayer, Run8.EndTransportPlayer, msg, null);
		}

		/// <summary>
		/// Teleports the player to a track circuit.
		/// </summary>
		/// <param name="route">The sub-area to teleport to.</param>
		/// <param name="tc">The track circuit to teleport to.</parma>
		public Task TransportPlayerToBlockAsync(int route, int tc) {
			TransportPlayerToBlockMessage msg;
			msg.Route = route;
			msg.BlockDetectorID = tc;
			return Task.Factory.FromAsync(Run8.BeginTransportPlayerToBlock, Run8.EndTransportPlayerToBlock, msg, null);
		}

		/// <summary>
		/// The communication link.
		/// </summary>
		private readonly IRun8 Run8;
	}
}
