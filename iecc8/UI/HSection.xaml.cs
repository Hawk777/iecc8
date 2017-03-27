using System.Windows;

namespace Iecc8.UI {
	/// <summary>
	/// Interaction logic for HSection.xaml
	/// </summary>
	public partial class HSection : TrackSection {
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
		public static readonly DependencyProperty TCBLeftProperty = DependencyProperty.Register(nameof(TCBLeft), typeof(bool), typeof(HSection));

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
		public static readonly DependencyProperty TCBRightProperty = DependencyProperty.Register(nameof(TCBRight), typeof(bool), typeof(HSection));

		/// <summary>
		/// Constructs a new HSection.
		/// </summary>
		public HSection() {
			InitializeComponent();
		}

		/// <summary>
		/// Refreshes the appearance of this object.
		/// </summary>
		protected override void Update() {
			Rectangle.Fill = RenderColour;
		}
	}
}
