using System.Windows;
using System.Windows.Controls;

namespace Iecc8.UI {
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
		public static readonly DependencyProperty TCBLeftProperty = DependencyProperty.Register(nameof(TCBLeft), typeof(bool), typeof(NonTCHSection));

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
		public static readonly DependencyProperty TCBRightProperty = DependencyProperty.Register(nameof(TCBRight), typeof(bool), typeof(NonTCHSection));

		/// <summary>
		/// Constructs a new NonTCHSection.
		/// </summary>
		public NonTCHSection() {
			InitializeComponent();
		}
	}
}
