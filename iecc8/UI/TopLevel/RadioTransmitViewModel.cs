using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Iecc8.UI.TopLevel {
	public class RadioTransmitViewModel : BindableBase, ICommand {
		public event EventHandler CanExecuteChanged;

		/// <summary>
		/// Which channel to send the message on.
		/// </summary>
		public string Channel {
			get {
				return ChannelImpl.ToString();
			}
			set {
				uint newChannel;
				if (uint.TryParse(value, out newChannel)) {
					if (newChannel >= 100) {
						newChannel = uint.MaxValue;
					}
				} else {
					newChannel = uint.MaxValue;
				}
				if (SetProperty(ref ChannelImpl, newChannel)) {
					CanExecuteChanged(this, EventArgs.Empty);
				}
				if (newChannel == uint.MaxValue) {
					throw new ApplicationException();
				}
			}
		}

		/// <summary>
		/// The message to send.
		/// </summary>
		public string Message {
			get {
				return MessageImpl;
			}
			set {
				if (SetProperty(ref MessageImpl, value)) {
					CanExecuteChanged(this, EventArgs.Empty);
				}
			}
		}

		bool ICommand.CanExecute(object parameter) {
			return Message.Length > 0 && ChannelImpl < 100;
		}

		void ICommand.Execute(object parameter) {
			World.SendRadioMessage(ChannelImpl, MessageImpl);
			Message = string.Empty;
		}

		/// <summary>
		/// Constructs a new RadioTransmitBarViewModel.
		/// </summary>
		/// <param name="world">The world into which to transmit messages.</param>
		public RadioTransmitViewModel(World.World world) {
			Debug.Assert(world != null);
			World = world;
			ChannelImpl = 0;
			MessageImpl = string.Empty;
		}

		private readonly World.World World;
		private uint ChannelImpl;
		private string MessageImpl;
	}
}
