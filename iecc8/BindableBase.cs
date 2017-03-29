using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Iecc8 {
	/// <summary>
	/// An implementation of INotifyPropertyChanged which minimizes code needed in individual properties.
	/// </summary>
	/// <remarks>
	/// This class came from http://danrigby.com/2015/09/12/inotifypropertychanged-the-net-4-6-way/.
	/// </remarks>
	public abstract class BindableBase : INotifyPropertyChanged {
		/// <summary>
		/// Emitted when the value of a property changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Sets a property.
		/// </summary>
		/// <remarks>
		/// If the property already has the requested value, nothing happens. The <c>object.Equals</c> static method is used to check for equality. If the property's value changed, a notification is emitted.
		/// </remarks>
		/// <typeparam name="T">The type of property being set.</typeparam>
		/// <param name="storage">The storage field holding the property's value.</param>
		/// <param name="value">The new value being set.</param>
		/// <param name="property">The name of the property being modified.</param>
		/// <returns><c>true</c> if the property changed value, or <c>false</c> if not.</returns>
		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string property = null) {
			if (Equals(storage, value)) {
				return false;
			} else {
				storage = value;
				EmitPropertyChanged(property);
				return true;
			}
		}

		/// <summary>
		/// Emits a property change event.
		/// </summary>
		/// <param name="property">The name of the property whose value changed.</param>
		protected void EmitPropertyChanged(string property) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
		}
	}
}
