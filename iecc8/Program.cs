using Iecc8.Messages;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Iecc8 {
	public static class Program {
		private const string ADDRESS = "net.tcp://localhost:15192/Run8";

		public static void Main(string[] args) {
			Task.Run(Run).Wait();
		}

		private static async Task Run() {
			NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
			DispatcherCallbacks cbs = new DispatcherCallbacks();
			InstanceContext ic = new InstanceContext(cbs);
			using (DuplexChannelFactory<IRun8> factory = new DuplexChannelFactory<IRun8>(ic, binding, ADDRESS)) {
				Run8Wrapper obj = new Run8Wrapper(factory.CreateChannel());
				await obj.DispatcherConnectedAsync();
				int defaultSubArea = 0;
				for (;;) {
					string line = Console.ReadLine();
					if (line.Length >= 3) {
						char objectType = line[0];
						bool hasAction = objectType != 'S';
						char action = hasAction ? line[line.Length - 1] : '\0';
						int target;
						bool targetOK = int.TryParse(line.Substring(1, line.Length - (hasAction ? 2 : 1)), out target);
						if (targetOK && (objectType == 'S')) {
							defaultSubArea = target;
						} else if (targetOK && (objectType == 's') && ((action == 's') || (action == 'p') || (action == 'f') || (action == 'F'))) {
							await obj.ChangeSignalAsync(defaultSubArea, target, (action == 's') ? ESignalIndication.Stop : (action == 'p') ? ESignalIndication.Proceed : (action == 'f') ? ESignalIndication.Fleet : ESignalIndication.FlagBy);
						} else if (targetOK && (objectType == 'p') && ((action == 'n') || (action == 'r') || (action == 'u') || (action == 'l'))) {
							await obj.ThrowSwitchAsync(defaultSubArea, target, (action == 'n') ? ESwitchState.Normal : (action == 'r') ? ESwitchState.Reversed : (action == 'u') ? ESwitchState.Unlocked : ESwitchState.Locked);
						} else if (targetOK && (objectType == 't') && (action == 'b')) {
							await obj.TransportPlayerToBlockAsync(defaultSubArea, target);
						} else if (targetOK && (objectType == 't') && (action == 'p')) {
							await obj.TransportPlayerAsync(defaultSubArea, target);
						} else {
							Console.WriteLine("Usage:");
							Console.WriteLine("S123 - set subsequent commands to target sub-area 123");
							Console.WriteLine("p123n - throw points 123 to normal");
							Console.WriteLine("p123r - throw points 123 to reverse");
							Console.WriteLine("p123u - unlock points 123 for hand cranking");
							Console.WriteLine("p123l - lock points 123 for power use");
							Console.WriteLine("s123s - set signal 123 to stop");
							Console.WriteLine("s123p - set signal 123 to proceed");
							Console.WriteLine("s123f - set signal 123 to fleet");
							Console.WriteLine("s123F - set signal 123 to flag-by");
							Console.WriteLine("t123b - teleport to block 123");
							Console.WriteLine("t123p - teleport to points 123");
						}
					}
				}
			}
		}
	}
}
