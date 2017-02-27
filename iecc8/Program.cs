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
				for (;;) {
					System.Threading.Thread.Sleep(5000);
				}
			}
		}
	}
}
