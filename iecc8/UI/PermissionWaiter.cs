using Iecc8.Messages;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Iecc8.UI {
	/// <summary>
	/// Enables waiting until external dispatcher permission has been granted by Run8.
	/// </summary>
	public class PermissionWaiter : AbstractDispatcher {
		/// <summary>
		/// Captures received messages and waits until dispatcher permission has been granted.
		/// </summary>
		/// <param name="proxy">The received messages proxy.</param>
		/// <param name="cancelToken">The cancellation token.</param>
		public static async Task WaitForPermissionAsync(DispatcherProxy proxy, CancellationToken cancelToken = default(CancellationToken)) {
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			proxy.SetTarget(new PermissionWaiter(tcs));
			try {
				using (cancelToken.Register(() => tcs.TrySetCanceled())) {
					await tcs.Task;
				}
			} finally {
				proxy.SetTarget(null);
			}
		}

		private readonly TaskCompletionSource<object> CompletionSource;

		private PermissionWaiter(TaskCompletionSource<object> completionSource) {
			Debug.Assert(completionSource != null);
			CompletionSource = completionSource;
		}

		public override void PermissionUpdate(DispatcherPermissionMessage pMessage) {
			if (pMessage.Permission == EDispatcherPermission.Granted || pMessage.Permission == EDispatcherPermission.Observer) {
				CompletionSource.TrySetResult(null);
			}
		}
	}
}
