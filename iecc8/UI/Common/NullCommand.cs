using System;
using System.Windows.Input;

namespace Iecc8.UI.Common {
	public class NullCommand : ICommand {
		public event EventHandler CanExecuteChanged {
			add {
			}
			remove {
			}
		}

		public bool CanExecute(object parameter) {
			return false;
		}

		public void Execute(object parameter) {
			throw new NotImplementedException();
		}
	}
}
