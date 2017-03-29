using Iecc8.Messages;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Iecc8.UI {
	/// <summary>
	/// Enables waiting until external dispatcher permission has been granted by Run8.
	/// </summary>
	public class SubAreaNumberWaiter : AbstractDispatcher {
		/// <summary>
		/// Captures received messages and waits until dispatcher permission has been granted.
		/// </summary>
		/// <param name="proxy">The received messages proxy.</param>
		/// <param name="cancelToken">The cancellation token.</param>
		public static async Task<int> WaitForSubAreaNumberAsync(DispatcherProxy proxy, CancellationToken cancelToken = default(CancellationToken)) {
			TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
			proxy.SetTarget(new SubAreaNumberWaiter(tcs));
			try {
				using (cancelToken.Register(() => tcs.TrySetCanceled())) {
					return await tcs.Task;
				}
			} finally {
				proxy.SetTarget(null);
			}
		}

		private readonly TaskCompletionSource<int> CompletionSource;

		private SubAreaNumberWaiter(TaskCompletionSource<int> completionSource) {
			Debug.Assert(completionSource != null);
			CompletionSource = completionSource;
		}

		public override void SetOccupiedBlocks(OccupiedBlocksMessage pMessage) {
			CompletionSource.TrySetResult(pMessage.Route);
		}
	}
}
