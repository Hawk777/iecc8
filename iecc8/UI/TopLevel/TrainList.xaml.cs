using Iecc8.UI.Common;
using Iecc8.World;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Iecc8.UI.TopLevel {
	/// <summary>
	/// Interaction logic for TrainList.xaml
	/// </summary>
	public partial class TrainList : ListView {
		public TrainList() {
			InitializeComponent();
		}

		private void SortByLoco(object sender, RoutedEventArgs e) {
			SortBy("Company", "LocoNumber");
		}

		private void SortByTag(object sender, RoutedEventArgs e) {
			SortBy("Tag");
		}

		private void SortByLocation(object sender, RoutedEventArgs e) {
			SortBy("SubArea", "Location");
		}

		private void SortBySpeed(object sender, RoutedEventArgs e) {
			SortBy("Speed");
		}

		private void SortByEngineer(object sender, RoutedEventArgs e) {
			SortBy("EngineerType", "EngineerName");
		}

		private void SortBy(params string[] keys) {
			SortDescriptionCollection desc = Items.SortDescriptions;
			desc.Clear();
			foreach (string key in keys) {
				desc.Add(new SortDescription(key, ListSortDirection.Ascending));
			}
		}

		private void OnDoubleClick(object sender, MouseButtonEventArgs e) {
			List<Train> trains = new List<Train>();
			trains.Add((Train) SelectedItem);
			MainViewModel vm = (MainViewModel) DataContext;
			vm.ShowTrainList = false;
			vm.SetDetailTrains(trains);
		}
	}
}
