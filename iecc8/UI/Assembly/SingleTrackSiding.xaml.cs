using Iecc8.UI.Equipment;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Iecc8.UI.Assembly {
	/// <summary>
	/// Interaction logic for SingleTrackSiding.xaml
	/// </summary>
	public partial class SingleTrackSiding : Grid {
		/// <summary>
		/// The name of this siding.
		/// </summary>
		[Category("Track"), Description("Gets or sets the name shown above the assemhly.")]
		public string SidingName {
			get {
				return (string) GetValue(SidingNameProperty);
			}
			set {
				SetValue(SidingNameProperty, value);
			}
		}

		/// <summary>
		/// The siding name property.
		/// </summary>
		public static readonly DependencyProperty SidingNameProperty = DependencyProperty.Register(nameof(SidingName), typeof(string), typeof(SingleTrackSiding));

		/// <summary>
		/// Which sub-area this signal is in.
		/// </summary>
		[Category("Track"), Description("Gets or sets the ID number of the subdivision containing all the track elements in this siding and its turnouts.")]
		public ushort SubAreaID {
			get {
				return (ushort) GetValue(SubAreaIDProperty);
			}
			set {
				SetValue(SubAreaIDProperty, value);
			}
		}

		/// <summary>
		/// The sub-area ID property.
		/// </summary>
		public static readonly DependencyProperty SubAreaIDProperty = DependencyProperty.Register(nameof(SubAreaID), typeof(ushort), typeof(SingleTrackSiding));

		/// <summary>
		/// The ID number of the signal on the single track at the left end.
		/// </summary>
		[Category("Track"), Description("Gets or sets the ID number of the signal on the single track at the left end.")]
		public short LeftSingleSignal {
			get {
				return (short) GetValue(LeftSignalSignalProperty);
			}
			set {
				SetValue(LeftSignalSignalProperty, value);
			}
		}

		/// <summary>
		/// The left single signal ID property.
		/// </summary>
		public static readonly DependencyProperty LeftSignalSignalProperty = DependencyProperty.Register(nameof(LeftSingleSignal), typeof(short), typeof(SingleTrackSiding));

		/// <summary>
		/// The ID number of the signal on the mainline at the left end.
		/// </summary>
		[Category("Track"), Description("Gets or sets the ID number of the signal on the mainline at the left end.")]
		public short LeftMainSignal {
			get {
				return (short) GetValue(LeftMainSignalProperty);
			}
			set {
				SetValue(LeftMainSignalProperty, value);
			}
		}

		/// <summary>
		/// The left main signal ID property.
		/// </summary>
		public static readonly DependencyProperty LeftMainSignalProperty = DependencyProperty.Register(nameof(LeftMainSignal), typeof(short), typeof(SingleTrackSiding));

		/// <summary>
		/// The ID number of the signal on the siding at the left end.
		/// </summary>
		[Category("Track"), Description("Gets or sets the ID number of the signal on the siding at the left end.")]
		public short LeftSidingSignal {
			get {
				return (short) GetValue(LeftSidingSignalProperty);
			}
			set {
				SetValue(LeftSidingSignalProperty, value);
			}
		}

		/// <summary>
		/// The left siding signal ID property.
		/// </summary>
		public static readonly DependencyProperty LeftSidingSignalProperty = DependencyProperty.Register(nameof(LeftSidingSignal), typeof(short), typeof(SingleTrackSiding));

		/// <summary>
		/// The ID number of the signal on the single track at the right end.
		/// </summary>
		[Category("Track"), Description("Gets or sets the ID number of the signal on the single track at the right end.")]
		public short RightSingleSignal {
			get {
				return (short) GetValue(RightSingleSignalProperty);
			}
			set {
				SetValue(RightSingleSignalProperty, value);
			}
		}

		/// <summary>
		/// The right single signal ID property.
		/// </summary>
		public static readonly DependencyProperty RightSingleSignalProperty = DependencyProperty.Register(nameof(RightSingleSignal), typeof(short), typeof(SingleTrackSiding));

		/// <summary>
		/// The ID number of the signal on the mainline at the right end.
		/// </summary>
		[Category("Track"), Description("Gets or sets the ID number of the signal on the mainline at the right end.")]
		public short RightMainSignal {
			get {
				return (short) GetValue(RightMainSignalProperty);
			}
			set {
				SetValue(RightMainSignalProperty, value);
			}
		}

		/// <summary>
		/// The right main signal ID property.
		/// </summary>
		public static readonly DependencyProperty RightMainSignalProperty = DependencyProperty.Register(nameof(RightMainSignal), typeof(short), typeof(SingleTrackSiding));

		/// <summary>
		/// The ID number of the signal on the siding at the right end.
		/// </summary>
		[Category("Track"), Description("Gets or sets the ID number of the signal on the siding at the right end.")]
		public short RightSidingSignal {
			get {
				return (short) GetValue(RightSidingSignalProperty);
			}
			set {
				SetValue(RightSidingSignalProperty, value);
			}
		}

		/// <summary>
		/// The right siding signal ID property.
		/// </summary>
		public static readonly DependencyProperty RightSidingSignalProperty = DependencyProperty.Register(nameof(RightSidingSignal), typeof(short), typeof(SingleTrackSiding));

		/// <summary>
		/// The track circuit covering the left points.
		/// </summary>
		[Category("Track"), Description("Gets or sets the ID number of the track circuit covering the left points.")]
		public int LeftTrackCircuit {
			get {
				return (int) GetValue(LeftTrackCircuitProperty);
			}
			set {
				SetValue(LeftTrackCircuitProperty, value);
			}
		}

		/// <summary>
		/// The left track circuit property.
		/// </summary>
		public static readonly DependencyProperty LeftTrackCircuitProperty = DependencyProperty.Register(nameof(LeftTrackCircuit), typeof(int), typeof(SingleTrackSiding));

		/// <summary>
		/// The track circuit covering the right points.
		/// </summary>
		[Category("Track"), Description("Gets or sets the ID number of the track circuit covering the right points.")]
		public int RightTrackCircuit {
			get {
				return (int) GetValue(RightTrackCircuitProperty);
			}
			set {
				SetValue(RightTrackCircuitProperty, value);
			}
		}

		/// <summary>
		/// The right track circuit property.
		/// </summary>
		public static readonly DependencyProperty RightTrackCircuitProperty = DependencyProperty.Register(nameof(RightTrackCircuit), typeof(int), typeof(SingleTrackSiding));

		/// <summary>
		/// The track circuit covering the mainline.
		/// </summary>
		[Category("Track"), Description("Gets or sets the ID number of the track circuit covering the mainline.")]
		public int MainTrackCircuit {
			get {
				return (int) GetValue(MainTrackCircuitProperty);
			}
			set {
				SetValue(MainTrackCircuitProperty, value);
			}
		}

		/// <summary>
		/// The mainline track circuit property.
		/// </summary>
		public static readonly DependencyProperty MainTrackCircuitProperty = DependencyProperty.Register(nameof(MainTrackCircuit), typeof(int), typeof(SingleTrackSiding));

		/// <summary>
		/// The track circuit covering the siding.
		/// </summary>
		[Category("Track"), Description("Gets or sets the ID number of the track circuit covering the siding.")]
		public int SidingTrackCircuit {
			get {
				return (int) GetValue(SidingTrackCircuitProperty);
			}
			set {
				SetValue(SidingTrackCircuitProperty, value);
			}
		}

		/// <summary>
		/// The siding track circuit property.
		/// </summary>
		public static readonly DependencyProperty SidingTrackCircuitProperty = DependencyProperty.Register(nameof(SidingTrackCircuit), typeof(int), typeof(SingleTrackSiding));

		/// <summary>
		/// The ID number of the points at the left end.
		/// </summary>
		[Category("Track"), Description("Gets or sets the ID number of the points at the left end.")]
		public short LeftPoints {
			get {
				return (short) GetValue(LeftPointsProperty);
			}
			set {
				SetValue(LeftPointsProperty, value);
			}
		}

		/// <summary>
		/// The left points ID property.
		/// </summary>
		public static readonly DependencyProperty LeftPointsProperty = DependencyProperty.Register(nameof(LeftPoints), typeof(short), typeof(SingleTrackSiding), new PropertyMetadata((short) -1, OnPointsChanged));

		/// <summary>
		/// The ID number of the points at the right end.
		/// </summary>
		[Category("Track"), Description("Gets or sets the ID number of the points at the right end.")]
		public short RightPoints {
			get {
				return (short) GetValue(RightPointsProperty);
			}
			set {
				SetValue(RightPointsProperty, value);
			}
		}

		/// <summary>
		/// The right points ID property.
		/// </summary>
		public static readonly DependencyProperty RightPointsProperty = DependencyProperty.Register(nameof(RightPoints), typeof(short), typeof(SingleTrackSiding), new PropertyMetadata((short) -1, OnPointsChanged));

		/// <summary>
		/// Whether to flip the siding upside-down (to make an A instead of a V).
		/// </summary>
		[Category("Track"), Description("Gets or sets whether the entire assembly is flipped upside-down.")]
		public bool Invert {
			get {
				return (bool) GetValue(InvertProperty);
			}
			set {
				SetValue(InvertProperty, value);
			}
		}

		/// <summary>
		/// The invert property.
		/// </summary>
		public static readonly DependencyProperty InvertProperty = DependencyProperty.Register(nameof(Invert), typeof(bool), typeof(SingleTrackSiding), new PropertyMetadata(false, OnInvertChanged));

		/// <summary>
		/// A SectionPointPosition list for the left points being normal.
		/// </summary>
		[Category("Internal"), Description("This property is used internally; do not set it.")]
		public List<SectionPointPosition> LeftNormalSPP {
			get {
				return (List<SectionPointPosition>) GetValue(LeftNormalSPPProperty);
			}
			set {
				SetValue(LeftNormalSPPProperty, value);
			}
		}

		/// <summary>
		/// The left normal SPP property.
		/// </summary>
		public static readonly DependencyProperty LeftNormalSPPProperty = DependencyProperty.Register(nameof(LeftNormalSPP), typeof(List<SectionPointPosition>), typeof(SingleTrackSiding));

		/// <summary>
		/// A SectionPointPosition list for the left points being reversed.
		/// </summary>
		[Category("Internal"), Description("This property is used internally; do not set it.")]
		public List<SectionPointPosition> LeftReverseSPP {
			get {
				return (List<SectionPointPosition>) GetValue(LeftReverseSPPProperty);
			}
			set {
				SetValue(LeftReverseSPPProperty, value);
			}
		}

		/// <summary>
		/// The left reversed SPP property.
		/// </summary>
		public static readonly DependencyProperty LeftReverseSPPProperty = DependencyProperty.Register(nameof(LeftReverseSPP), typeof(List<SectionPointPosition>), typeof(SingleTrackSiding));

		/// <summary>
		/// A SectionPointPosition list for the right points being normal.
		/// </summary>
		[Category("Internal"), Description("This property is used internally; do not set it.")]
		public List<SectionPointPosition> RightNormalSPP {
			get {
				return (List<SectionPointPosition>) GetValue(RightNormalSPPProperty);
			}
			set {
				SetValue(RightNormalSPPProperty, value);
			}
		}

		/// <summary>
		/// The right normal SPP property.
		/// </summary>
		public static readonly DependencyProperty RightNormalSPPProperty = DependencyProperty.Register(nameof(RightNormalSPP), typeof(List<SectionPointPosition>), typeof(SingleTrackSiding));

		/// <summary>
		/// A SectionPointPosition list for the right points being reversed.
		/// </summary>
		[Category("Internal"), Description("This property is used internally; do not set it.")]
		public List<SectionPointPosition> RightReverseSPP {
			get {
				return (List<SectionPointPosition>) GetValue(RightReverseSPPProperty);
			}
			set {
				SetValue(RightReverseSPPProperty, value);
			}
		}

		/// <summary>
		/// The right reversed SPP property.
		/// </summary>
		public static readonly DependencyProperty RightReverseSPPProperty = DependencyProperty.Register(nameof(RightReverseSPP), typeof(List<SectionPointPosition>), typeof(SingleTrackSiding));

		/// <summary>
		/// The transform to apply based on the Invert property.
		/// </summary>
		[Category("Internal"), Description("This property is used internally; do not set it.")]
		public ScaleTransform InvertTransform {
			get {
				return (ScaleTransform) GetValue(InvertTransformProperty);
			}
			set {
				SetValue(InvertTransformProperty, value);
			}
		}

		/// <summary>
		/// The invert transform property.
		/// </summary>
		public static readonly DependencyProperty InvertTransformProperty = DependencyProperty.Register(nameof(InvertTransform), typeof(ScaleTransform), typeof(SingleTrackSiding));

		/// <summary>
		/// The transform to apply based on the Invert property for objects that need to be horizontally flipped.
		/// </summary>
		[Category("Internal"), Description("This property is used internally; do not set it.")]
		public ScaleTransform HFlipInvertTransform {
			get {
				return (ScaleTransform) GetValue(HFlipInvertTransformProperty);
			}
			set {
				SetValue(HFlipInvertTransformProperty, value);
			}
		}

		/// <summary>
		/// The horizontally flipped invert transform property.
		/// </summary>
		public static readonly DependencyProperty HFlipInvertTransformProperty = DependencyProperty.Register(nameof(HFlipInvertTransform), typeof(ScaleTransform), typeof(SingleTrackSiding), new PropertyMetadata(new ScaleTransform(-1, 1)));

		/// <summary>
		/// The transform to apply based on the Invert property for objects that need to be horizontally and vertically flipped.
		/// </summary>
		[Category("Internal"), Description("This property is used internally; do not set it.")]
		public ScaleTransform HFlipVFlipInvertTransform {
			get {
				return (ScaleTransform) GetValue(HFlipVFlipInvertTransformProperty);
			}
			set {
				SetValue(HFlipVFlipInvertTransformProperty, value);
			}
		}

		/// <summary>
		/// The horizontally and vertically flipped invert transform property.
		/// </summary>
		public static readonly DependencyProperty HFlipVFlipInvertTransformProperty = DependencyProperty.Register(nameof(HFlipVFlipInvertTransform), typeof(ScaleTransform), typeof(SingleTrackSiding), new PropertyMetadata(new ScaleTransform(-1, -1)));

		/// <summary>
		/// Which grid row the left single signal appears on.
		/// </summary>
		[Category("Internal"), Description("This property is used internally; do not set it.")]
		public int LeftSingleSignalGridRow {
			get {
				return (int) GetValue(LeftSingleSignalGridRowProperty);
			}
			set {
				SetValue(LeftSingleSignalGridRowProperty, value);
			}
		}

		/// <summary>
		/// The left single signal grid row property.
		/// </summary>
		public static readonly DependencyProperty LeftSingleSignalGridRowProperty = DependencyProperty.Register(nameof(LeftSingleSignalGridRow), typeof(int), typeof(SingleTrackSiding), new PropertyMetadata(2));

		/// <summary>
		/// Which grid row the right single signal appears on.
		/// </summary>
		[Category("Internal"), Description("This property is used internally; do not set it.")]
		public int RightSingleSignalGridRow {
			get {
				return (int) GetValue(RightSingleSignalGridRowProperty);
			}
			set {
				SetValue(RightSingleSignalGridRowProperty, value);
			}
		}

		/// <summary>
		/// The right single signal grid row property.
		/// </summary>
		public static readonly DependencyProperty RightSingleSignalGridRowProperty = DependencyProperty.Register(nameof(RightSingleSignalGridRow), typeof(int), typeof(SingleTrackSiding), new PropertyMetadata(0));

		/// <summary>
		/// Constructs a new SingleTrackSiding.
		/// </summary>
		public SingleTrackSiding() {
			InitializeComponent();
		}

		/// <summary>
		/// Invoked when a points number changes.
		/// </summary>
		/// <param name="d">The object whose value changed.</param>
		/// <param name="e">Details of the change.</param>
		private static void OnPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			SingleTrackSiding obj = (SingleTrackSiding) d;

			SectionPointPosition leftNormalSPP = new SectionPointPosition(obj.LeftPoints, false);
			SectionPointPosition leftReverseSPP = new SectionPointPosition(obj.LeftPoints, true);
			SectionPointPosition rightNormalSPP = new SectionPointPosition(obj.RightPoints, false);
			SectionPointPosition rightReverseSPP = new SectionPointPosition(obj.RightPoints, true);

			obj.LeftNormalSPP = new List<SectionPointPosition> { leftNormalSPP };
			obj.LeftReverseSPP = new List<SectionPointPosition> { leftReverseSPP };
			obj.RightNormalSPP = new List<SectionPointPosition> { rightNormalSPP };
			obj.RightReverseSPP = new List<SectionPointPosition> { rightReverseSPP };
		}

		/// <summary>
		/// Invoked when the invert property changes.
		/// </summary>
		/// <param name="d">The object whose value changed.</param>
		/// <param name="e">Details of the change.</param>
		private static void OnInvertChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			SingleTrackSiding obj = (SingleTrackSiding) d;
			if (obj.Invert) {
				obj.InvertTransform = new ScaleTransform(1, -1);
				obj.HFlipInvertTransform = new ScaleTransform(-1, -1);
				obj.HFlipVFlipInvertTransform = new ScaleTransform(-1, 1);
				obj.LeftSingleSignalGridRow = 0;
				obj.RightSingleSignalGridRow = 2;
			} else {
				obj.InvertTransform = new ScaleTransform(1, 1);
				obj.HFlipInvertTransform = new ScaleTransform(-1, 1);
				obj.HFlipVFlipInvertTransform = new ScaleTransform(-1, -1);
				obj.LeftSingleSignalGridRow = 2;
				obj.RightSingleSignalGridRow = 0;
			}
		}
	}
}
