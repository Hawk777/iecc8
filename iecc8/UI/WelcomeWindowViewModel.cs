using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Iecc8.UI {
	/// <summary>
	/// The view-model for the welcome window.
	/// </summary>
	public class WelcomeWindowViewModel : BindableBase {
		/// <summary>
		/// The command executed when the user clicks the Connect button.
		/// </summary>
		private class ConnectCommandCls : ICommand {
			public event EventHandler CanExecuteChanged;
			private readonly WelcomeWindowViewModel Parent;
			private CancellationTokenSource CancelSource;

			/// <summary>
			/// Constructs a new ConnectCommandCls.
			/// </summary>
			/// <param name="parent">The main view model under which this command exists.</param>
			public ConnectCommandCls(WelcomeWindowViewModel parent) {
				Parent = parent;
			}

			/// <summary>
			/// Emits the CanExecuteChanged event.
			/// </summary>
			public void EmitCanExecuteChanged() {
				CanExecuteChanged(this, EventArgs.Empty);
			}

			/// <summary>
			/// Cancels the in-progress connection attempt.
			/// </summary>
			public void Cancel() {
				Debug.Assert(Parent.ConnectionProgress != EConnectionProgress.None && Parent.ConnectionProgress != EConnectionProgress.Cancelling);
				Parent.ConnectionProgress = EConnectionProgress.Cancelling;
				CancelSource.Cancel();
			}

			bool ICommand.CanExecute(object parameter) {
				return (Parent.ConnectionProgress == EConnectionProgress.None) && (Parent.ConnectToLocal || (Parent.RemoteHostname.Length > 0));
			}

			async void ICommand.Execute(object parameter) {
				CancelSource = new CancellationTokenSource();
				await ExecuteAsync((Window) parameter, CancelSource.Token);
			}

			/// <summary>
			/// The implementation of Execute.
			/// </summary>
			/// <param name="welcomeWindow">The welcome window, which is closed if login succeeds.</param>
			/// <param name="cancelToken">A token used to cancel connection.</param>
			private async Task ExecuteAsync(Window welcomeWindow, CancellationToken cancelToken) {
				Debug.Assert(Parent.ConnectionProgress == EConnectionProgress.None);

				// Create the objects.
				NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
				DispatcherProxy proxy = new DispatcherProxy();
				InstanceContext ic = new InstanceContext(proxy);
				string uri = BuildUri(Parent.ConnectToLocal ? "localhost" : Parent.RemoteHostname).ToString();
				DuplexChannelFactory<IRun8> dcf = new DuplexChannelFactory<IRun8>(ic, binding, uri);
				Parent.ConnectionProgress = EConnectionProgress.Connecting;
				dcf.Open();
				if (dcf.State == CommunicationState.Faulted) {
					Parent.LastErrorMessage = "Failed to connect.";
					Parent.ConnectionProgress = EConnectionProgress.None;
					return;
				}
				Run8Wrapper run8 = new Run8Wrapper(dcf.CreateChannel());

				// Send the DispatcherConnected message, allowing the operation to be cancelled.
				{
					Task connectTask = null;
					try {
						TaskCompletionSource<object> cancelTCS = new TaskCompletionSource<object>();
						connectTask = run8.DispatcherConnectedAsync();
						using (cancelToken.Register(() => cancelTCS.TrySetCanceled())) {
							await await Task.WhenAny(connectTask, cancelTCS.Task);
						}
					} catch (OperationCanceledException) {
						dcf.Abort();
						Parent.ConnectionProgress = EConnectionProgress.None;
						try {
							await connectTask;
						} catch (CommunicationException) {
							// Swallow.
						}
						return;
					} catch (CommunicationException exp) {
						dcf.Abort();
						Parent.LastErrorMessage = "Failed to connect: " + exp.Message;
						Parent.ConnectionProgress = EConnectionProgress.None;
						return;
					}
				}

				// Wait until Run8 grants us permission, allowing the operation to be cancelled.
				Parent.ConnectionProgress = EConnectionProgress.WaitingForPermission;
				try {
					await PermissionWaiter.WaitForPermissionAsync(proxy, cancelToken);
				} catch (OperationCanceledException) {
					dcf.Abort();
					Parent.ConnectionProgress = EConnectionProgress.None;
					return;
				}

				// Wait until Run8 sends us a message with a route number in it so we can look up in the database what data to load.
				Parent.ConnectionProgress = EConnectionProgress.WaitingForData;
				int route;
				try {
					route = await SubAreaNumberWaiter.WaitForSubAreaNumberAsync(proxy, cancelToken);
				} catch(OperationCanceledException) {
					dcf.Abort();
					Parent.ConnectionProgress = EConnectionProgress.None;
					return;
				}

				// Create the world.
				World.World world;
				try {
					world = new World.World(run8, route);
				} catch (World.World.UnrecognizedRegionException) {
					dcf.Abort();
					Parent.LastErrorMessage = "Unrecognized route ID " + route;
					Parent.ConnectionProgress = EConnectionProgress.None;
					return;
				}
				proxy.SetTarget(world);

				// Done! Launch the main window.
				Parent.ConnectionProgress = EConnectionProgress.None;
				MainWindow mw = new MainWindow(new MainViewModel(world));
				Application.Current.MainWindow = mw;
				mw.Show();
				welcomeWindow.Close();
			}
		}

		/// <summary>
		/// The command executed when the user clicks the Cancel button.
		/// </summary>
		private class CancelCommandCls : ICommand {
			public event EventHandler CanExecuteChanged;
			private readonly WelcomeWindowViewModel Parent;

			/// <summary>
			/// Constructs a new CancelCommandCls.
			/// </summary>
			/// <param name="parent">The main view model under which this command exists.</param>
			public CancelCommandCls(WelcomeWindowViewModel parent) {
				Parent = parent;
			}

			/// <summary>
			/// Emits the CanExecuteChanged event.
			/// </summary>
			public void EmitCanExecuteChanged() {
				CanExecuteChanged(this, EventArgs.Empty);
			}

			bool ICommand.CanExecute(object parameter) {
				return Parent.ConnectionProgress != EConnectionProgress.None && Parent.ConnectionProgress != EConnectionProgress.Cancelling;
			}

			void ICommand.Execute(object parameter) {
				Parent.ConnectCommandImpl.Cancel();
			}
		}

		/// <summary>
		/// Constructs a new WelcomeWindowViewModel.
		/// </summary>
		public WelcomeWindowViewModel() {
			ConnectCommandImpl = new ConnectCommandCls(this);
			CancelCommandImpl = new CancelCommandCls(this);
		}

		/// <summary>
		/// How far the system has gotten connecting to the Run8 server.
		/// </summary>
		public enum EConnectionProgress {
			None,
			Connecting,
			WaitingForPermission,
			WaitingForData,
			Cancelling,
		}

		/// <summary>
		/// Whether to connect to the local machine.
		/// </summary>
		public bool ConnectToLocal {
			get {
				return ConnectToLocalImpl;
			}
			set {
				if (SetProperty(ref ConnectToLocalImpl, value)) {
					EmitPropertyChanged(nameof(ConnectToRemote));
					EmitPropertyChanged(nameof(CanEditRemoteHostname));
					ConnectCommandImpl.EmitCanExecuteChanged();
				}
			}
		}

		/// <summary>
		/// Whether to connect to a remote machine.
		/// </summary>
		public bool ConnectToRemote {
			get {
				return !ConnectToLocal;
			}
			set {
				ConnectToLocal = !value;
			}
		}

		/// <summary>
		/// The hostname or IP address of the remote machine to connect to.
		/// </summary>
		public string RemoteHostname {
			get {
				return RemoteHostnameImpl;
			}
			set {
				if (RemoteHostnameImpl != value) {
					RemoteHostnameImpl = value;
					try {
						BuildUri(RemoteHostname);
					} catch (UriFormatException) {
						RemoteHostnameImpl = string.Empty;
						throw;
					} finally {
						EmitPropertyChanged(nameof(RemoteHostname));
						ConnectCommandImpl.EmitCanExecuteChanged();
					}
				}
			}
		}

		/// <summary>
		/// The command executed by the Connect button.
		/// </summary>
		public ICommand ConnectCommand {
			get {
				return ConnectCommandImpl;
			}
		}

		/// <summary>
		/// The command executed by the Cancel button.
		/// </summary>
		public ICommand CancelCommand {
			get {
				return CancelCommandImpl;
			}
		}

		/// <summary>
		/// The progress that has been made in connecting to Run8.
		/// </summary>
		public EConnectionProgress ConnectionProgress {
			get {
				return ConnectionProgressImpl;
			}
			private set {
				if (SetProperty(ref ConnectionProgressImpl, value)) {
					EmitPropertyChanged(nameof(ConnectionProgressString));
					EmitPropertyChanged(nameof(CanEditSettings));
					EmitPropertyChanged(nameof(CanEditRemoteHostname));
					ConnectCommandImpl.EmitCanExecuteChanged();
					CancelCommandImpl.EmitCanExecuteChanged();
				}
			}
		}

		/// <summary>
		/// The progress that has been made in conencting to Run8, as a string.
		/// </summary>
		public string ConnectionProgressString {
			get {
				switch (ConnectionProgress) {
					case EConnectionProgress.None:
						return LastErrorMessage ?? string.Empty;
					case EConnectionProgress.Connecting:
						return "Connecting";
					case EConnectionProgress.WaitingForPermission:
						return "Waiting for Permission";
					case EConnectionProgress.WaitingForData:
						return "Waiting for Data";
					case EConnectionProgress.Cancelling:
						return "Cancelling";
				}
				throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Indicates whether the connection settings can be edited right now.
		/// </summary>
		public bool CanEditSettings {
			get {
				return ConnectionProgress == EConnectionProgress.None;
			}
		}

		/// <summary>
		/// Indicates whether the remote hostname text field can be edited right now.
		/// </summary>
		public bool CanEditRemoteHostname {
			get {
				return CanEditSettings && ConnectToRemote;
			}
		}

		/// <summary>
		/// Storage for the ConnectToLocal property.
		/// </summary>
		private bool ConnectToLocalImpl = true;

		/// <summary>
		/// Storage for the RemoteHostname property.
		/// </summary>
		private string RemoteHostnameImpl = string.Empty;

		/// <summary>
		/// Storage for the ConnectCommand property.
		/// </summary>
		private readonly ConnectCommandCls ConnectCommandImpl;

		/// <summary>
		/// Storage for the CancelCommand property.
		/// </summary>
		private readonly CancelCommandCls CancelCommandImpl;

		private EConnectionProgress ConnectionProgressImpl = EConnectionProgress.None;

		private string LastErrorMessage = null;

		private static Uri BuildUri(string hostname) {
			UriBuilder builder = new UriBuilder();
			builder.Scheme = "net.tcp";
			builder.Host = hostname;
			builder.Port = 15192;
			builder.Path = "Run8";
			return builder.Uri;
		}
	}
}
