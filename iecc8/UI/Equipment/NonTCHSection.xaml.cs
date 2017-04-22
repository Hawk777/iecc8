using System.Windows;
using System.Windows.Controls;

namespace Iecc8.UI.Equipment {
	/// <summary>
	/// Interaction logic for NonTCHSection.xaml
	/// </summary>
	public partial class NonTCHSection : UserControl {
		/// <summary>
		/// Whether there is a TCB at the left end of this section.
		/// </summary>
		public bool TCBLeft {
			get {
				return (bool) GetValue(TCBLeftProperty);
			}
			set {
				SetValue(TCBLeftProperty, value);
			}
		}

		/// <summary>
		/// The TCB at left end property.
		/// </summary>
		public static readonly DependencyProperty TCBLeftProperty = DependencyProperty.Register(nameof(TCBLeft), typeof(bool), typeof(NonTCHSection), new PropertyMetadata(false, OnTCBPropertyChanged));

		/// <summary>
		/// Whether there is a TCB at the right end of this section.
		/// </summary>
		public bool TCBRight {
			get {
				return (bool) GetValue(TCBRightProperty);
			}
			set {
				SetValue(TCBRightProperty, value);
			}
		}

		/// <summary>
		/// The TCB at right end property.
		/// </summary>
		public static readonly DependencyProperty TCBRightProperty = DependencyProperty.Register(nameof(TCBRight), typeof(bool), typeof(NonTCHSection), new PropertyMetadata(false, OnTCBPropertyChanged));

		/// <summary>
		/// Constructs a new NonTCHSection.
		/// </summary>
		public NonTCHSection() {
			InitializeComponent();
		}

		/// <summary>
		/// Refreshes the shape of this object based on changes to its properties.
		/// </summary>
		private void UpdateShape() {
			Rectangle.Margin = new Thickness(TCBLeft ? 2 : 0, 0, TCBRight ? 2 : 0, 0);
		}

		/// <summary>
		/// Invoked when the TCB property changes on an object.
		/// </summary>
		/// <param name="d">The object whose value changed.</param>
		/// <param name="e">Details about the change.</param>
		private static void OnTCBPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			((NonTCHSection) d).UpdateShape();
		}
	}
}
