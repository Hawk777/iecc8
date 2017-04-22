using Iecc8.UI.Common;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Iecc8.UI.Equipment {
	/// <summary>
	/// An automatic or controlled signal.
	/// </summary>
	public class Signal : SignalBase {
		/// <summary>
		/// Constructs a new Signal.
		/// </summary>
		public Signal() {
			Stem = new SignalStem();
			HeadObjects = new List<SignalHead>();
			Grid.SetColumnSpan(this, 2);
		}

		/// <summary>
		/// Initializes the stem and heads.
		/// </summary>
		protected override void InitUI() {
			MainViewModel vm = DataContext as MainViewModel;
			if (vm != null) {
				Stem.Init(SignalObject);
				for (byte i = 0; i != SignalObject.Heads; ++i) {
					SignalHead head = new SignalHead(SignalObject, i, vm.BlinkClockSource);
					HeadObjects.Add(head);
				}
			} else {
				Stem.Init(null);
				SignalHead head = new SignalHead(null, 0, null);
				HeadObjects.Add(head);
			}

			Grid g = new Grid();
			{
				RowDefinition r = new RowDefinition();
				r.Height = new GridLength(0.0, GridUnitType.Auto);
				g.RowDefinitions.Add(r);
			}
			for (int i = 0; i != HeadObjects.Count + 1; ++i) {
				ColumnDefinition c = new ColumnDefinition();
				c.Width = new GridLength(0.0, GridUnitType.Auto);
				g.ColumnDefinitions.Add(c);
			}
			for (int i = 0; i != HeadObjects.Count; ++i) {
				SignalHead head = HeadObjects[i];
				g.Children.Add(head);
				Grid.SetColumn(head, i);
			}
			g.Children.Add(Stem);
			Grid.SetColumn(Stem, HeadObjects.Count);
			Content = g;

			Grid.SetColumnSpan(this, HeadObjects.Count + 1);
		}

		/// <summary>
		/// The signal stem.
		/// </summary>
		private readonly SignalStem Stem;

		/// <summary>
		/// The signal heads.
		/// </summary>
		private readonly List<SignalHead> HeadObjects;
	}
}
