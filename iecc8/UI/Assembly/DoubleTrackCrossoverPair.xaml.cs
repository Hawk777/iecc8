using Iecc8.UI.Equipment;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Iecc8.UI.Assembly {
	/// <summary>
	/// Interaction logic for DoubleTrackCrossoverPair.xaml
	/// </summary>
	public partial class DoubleTrackCrossoverPair : Grid {
		/// <summary>
		/// The name of this control point.
		/// </summary>
		public string CPName {
			get {
				return (string) GetValue(CPNameProperty);
			}
			set {
				SetValue(CPNameProperty, value);
			}
		}

		/// <summary>
		/// The control point name property.
		/// </summary>
		public static readonly DependencyProperty CPNameProperty = DependencyProperty.Register(nameof(CPName), typeof(string), typeof(DoubleTrackCrossoverPair));

		/// <summary>
		/// Which sub-area this signal is in.
		/// </summary>
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
		public static readonly DependencyProperty SubAreaIDProperty = DependencyProperty.Register(nameof(SubAreaID), typeof(ushort), typeof(DoubleTrackCrossoverPair));

		/// <summary>
		/// The ID number of the signal in the bottom-left corner.
		/// </summary>
		public short TopLeftSignal {
			get {
				return (short) GetValue(TopLeftSignalProperty);
			}
			set {
				SetValue(TopLeftSignalProperty, value);
			}
		}

		/// <summary>
		/// The bottom-left signal ID property.
		/// </summary>
		public static readonly DependencyProperty TopLeftSignalProperty = DependencyProperty.Register(nameof(TopLeftSignal), typeof(short), typeof(DoubleTrackCrossoverPair));

		/// <summary>
		/// The ID number of the signal in the top-right corner.
		/// </summary>
		public short TopRightSignal {
			get {
				return (short) GetValue(TopRightSignalProperty);
			}
			set {
				SetValue(TopRightSignalProperty, value);
			}
		}

		/// <summary>
		/// The top-right signal ID property.
		/// </summary>
		public static readonly DependencyProperty TopRightSignalProperty = DependencyProperty.Register(nameof(TopRightSignal), typeof(short), typeof(DoubleTrackCrossoverPair));

		/// <summary>
		/// The ID number of the signal in the bottom-left corner.
		/// </summary>
		public short BottomLeftSignal {
			get {
				return (short) GetValue(BottomLeftSignalProperty);
			}
			set {
				SetValue(BottomLeftSignalProperty, value);
			}
		}

		/// <summary>
		/// The bottom-left signal ID property.
		/// </summary>
		public static readonly DependencyProperty BottomLeftSignalProperty = DependencyProperty.Register(nameof(BottomLeftSignal), typeof(short), typeof(DoubleTrackCrossoverPair));

		/// <summary>
		/// The ID number of the signal in the bottom-right corner.
		/// </summary>
		public short BottomRightSignal {
			get {
				return (short) GetValue(BottomRightSignalProperty);
			}
			set {
				SetValue(BottomRightSignalProperty, value);
			}
		}

		/// <summary>
		/// The bottom-right signal ID property.
		/// </summary>
		public static readonly DependencyProperty BottomRightSignalProperty = DependencyProperty.Register(nameof(BottomRightSignal), typeof(short), typeof(DoubleTrackCrossoverPair));

		/// <summary>
		/// The track circuit that the top part of the assembly is part of.
		/// </summary>
		public int TopTrackCircuit {
			get {
				return (int) GetValue(TopTrackCircuitProperty);
			}
			set {
				SetValue(TopTrackCircuitProperty, value);
			}
		}

		/// <summary>
		/// The top track circuit property.
		/// </summary>
		public static readonly DependencyProperty TopTrackCircuitProperty = DependencyProperty.Register(nameof(TopTrackCircuit), typeof(int), typeof(DoubleTrackCrossoverPair));

		/// <summary>
		/// The track circuit that the bottom part of the assembly is part of.
		/// </summary>
		public int BottomTrackCircuit {
			get {
				return (int) GetValue(BottomTrackCircuitProperty);
			}
			set {
				SetValue(BottomTrackCircuitProperty, value);
			}
		}

		/// <summary>
		/// The bottom track circuit property.
		/// </summary>
		public static readonly DependencyProperty BottomTrackCircuitProperty = DependencyProperty.Register(nameof(BottomTrackCircuit), typeof(int), typeof(DoubleTrackCrossoverPair));

		/// <summary>
		/// The ID number of the left crossover points.
		/// </summary>
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
		public static readonly DependencyProperty LeftPointsProperty = DependencyProperty.Register(nameof(LeftPoints), typeof(short), typeof(DoubleTrackCrossoverPair), new PropertyMetadata((short) -1, OnPointsChanged));

		/// <summary>
		/// The ID number of the right crossover points.
		/// </summary>
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
		public static readonly DependencyProperty RightPointsProperty = DependencyProperty.Register(nameof(RightPoints), typeof(short), typeof(DoubleTrackCrossoverPair), new PropertyMetadata((short) -1, OnPointsChanged));

		/// <summary>
		/// Whether to flip the control point upside-down (to make an A instead of a V).
		/// </summary>
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
		public static readonly DependencyProperty InvertProperty = DependencyProperty.Register(nameof(Invert), typeof(bool), typeof(DoubleTrackCrossoverPair), new PropertyMetadata(false, OnInvertChanged));

		/// <summary>
		/// A SectionPointPosition list for the left points being normal.
		/// </summary>
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
		public static readonly DependencyProperty LeftNormalSPPProperty = DependencyProperty.Register(nameof(LeftNormalSPP), typeof(List<SectionPointPosition>), typeof(DoubleTrackCrossoverPair));

		/// <summary>
		/// A SectionPointPosition list for the left points being reversed.
		/// </summary>
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
		public static readonly DependencyProperty LeftReverseSPPProperty = DependencyProperty.Register(nameof(LeftReverseSPP), typeof(List<SectionPointPosition>), typeof(DoubleTrackCrossoverPair));

		/// <summary>
		/// A SectionPointPosition list for the right points being normal.
		/// </summary>
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
		public static readonly DependencyProperty RightNormalSPPProperty = DependencyProperty.Register(nameof(RightNormalSPP), typeof(List<SectionPointPosition>), typeof(DoubleTrackCrossoverPair));

		/// <summary>
		/// A SectionPointPosition list for the right points being reversed.
		/// </summary>
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
		public static readonly DependencyProperty RightReverseSPPProperty = DependencyProperty.Register(nameof(RightReverseSPP), typeof(List<SectionPointPosition>), typeof(DoubleTrackCrossoverPair));

		/// <summary>
		/// A SectionPointPosition list for the both points being normal.
		/// </summary>
		public List<SectionPointPosition> BothNormalSPP {
			get {
				return (List<SectionPointPosition>) GetValue(BothNormalSPPProperty);
			}
			set {
				SetValue(BothNormalSPPProperty, value);
			}
		}

		/// <summary>
		/// The both normal SPP property.
		/// </summary>
		public static readonly DependencyProperty BothNormalSPPProperty = DependencyProperty.Register(nameof(BothNormalSPP), typeof(List<SectionPointPosition>), typeof(DoubleTrackCrossoverPair));

		/// <summary>
		/// The transform to apply based on the Invert property.
		/// </summary>
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
		public static readonly DependencyProperty InvertTransformProperty = DependencyProperty.Register(nameof(InvertTransform), typeof(ScaleTransform), typeof(DoubleTrackCrossoverPair));

		/// <summary>
		/// The transform to apply based on the Invert property for objects that need to be horizontally flipped.
		/// </summary>
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
		public static readonly DependencyProperty HFlipInvertTransformProperty = DependencyProperty.Register(nameof(HFlipInvertTransform), typeof(ScaleTransform), typeof(DoubleTrackCrossoverPair), new PropertyMetadata(new ScaleTransform(-1, 1)));

		/// <summary>
		/// Constructs a new DoubleTrackCrossoverPair.
		/// </summary>
		public DoubleTrackCrossoverPair() {
			InitializeComponent();
		}

		/// <summary>
		/// Invoked when a points number changes.
		/// </summary>
		/// <param name="d">The object whose value changed.</param>
		/// <param name="e">Details of the change.</param>
		private static void OnPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			DoubleTrackCrossoverPair obj = (DoubleTrackCrossoverPair) d;

			SectionPointPosition leftNormalSPP = new SectionPointPosition(obj.LeftPoints, false);
			SectionPointPosition leftReverseSPP = new SectionPointPosition(obj.LeftPoints, true);
			SectionPointPosition rightNormalSPP = new SectionPointPosition(obj.RightPoints, false);
			SectionPointPosition rightReverseSPP = new SectionPointPosition(obj.RightPoints, true);

			obj.LeftNormalSPP = new List<SectionPointPosition> { leftNormalSPP };
			obj.LeftReverseSPP = new List<SectionPointPosition> { leftReverseSPP };
			obj.RightNormalSPP = new List<SectionPointPosition> { rightNormalSPP };
			obj.RightReverseSPP = new List<SectionPointPosition> { rightReverseSPP };
			obj.BothNormalSPP = new List<SectionPointPosition> { leftNormalSPP, rightNormalSPP };
		}

		/// <summary>
		/// Invoked when the invert property changes.
		/// </summary>
		/// <param name="d">The object whose value changed.</param>
		/// <param name="e">Details of the change.</param>
		private static void OnInvertChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			DoubleTrackCrossoverPair obj = (DoubleTrackCrossoverPair) d;
			if (obj.Invert) {
				obj.InvertTransform = new ScaleTransform(1, -1);
				obj.HFlipInvertTransform = new ScaleTransform(-1, -1);
			} else {
				obj.InvertTransform = new ScaleTransform(1, 1);
				obj.HFlipInvertTransform = new ScaleTransform(-1, 1);
			}
		}
	}
}
