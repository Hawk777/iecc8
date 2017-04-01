using Iecc8.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Iecc8.UI {
	public class TrainDetailViewModel : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// The currently selected train.
		/// </summary>
		public Train SelectedTrain {
			get {
				if (Trains != null) {
					return Trains[(int) SelectedTrainIndex];
				} else {
					return null;
				}
			}
		}

		/// <summary>
		/// The previous train command.
		/// </summary>
		public ICommand PrevCommand {
			get {
				return PrevCommandImpl;
			}
		}

		/// <summary>
		/// The previous train command.
		/// </summary>
		public ICommand NextCommand {
			get {
				return NextCommandImpl;
			}
		}

		/// <summary>
		/// Constructs a new TrainDetailViewModel.
		/// </summary>
		public TrainDetailViewModel() {
			PrevCommandImpl = new SwitchTrainCommand(this, true);
			NextCommandImpl = new SwitchTrainCommand(this, false);
		}

		/// <summary>
		/// Sets which trains are shown in the detail area.
		/// </summary>
		/// <param name="trains">The trains to show.</param>
		public void SetTrains(IReadOnlyList<Train> trains) {
			Trains = trains;
			SelectedTrainIndex = 0;
			EmitPropertyChanged(nameof(SelectedTrain));
			PrevCommandImpl.EmitCanExecuteChanged();
			NextCommandImpl.EmitCanExecuteChanged();
		}

		/// <summary>
		/// A command to shift up or down between trains.
		/// </summary>
		private class SwitchTrainCommand : ICommand {
			public event EventHandler CanExecuteChanged;

			/// <summary>
			/// Constructs a new SwitchTrainCommand.
			/// </summary>
			/// <param name="vm">The view model to live in.</param>
			/// <param name="decrement"><c>true</c> if this command moves to the previous train instead of the next train.</param>
			public SwitchTrainCommand(TrainDetailViewModel vm, bool decrement) {
				VM = vm;
				Decrement = decrement;
			}

			public bool CanExecute(object parameter) {
				if (Decrement) {
					return (VM.Trains != null) && (VM.SelectedTrainIndex > 0);
				} else {
					return (VM.Trains != null) && (VM.SelectedTrainIndex + 1 < VM.Trains.Count);
				}
			}

			public void Execute(object parameter) {
				if (Decrement) {
					VM.SetSelectedTrain(VM.SelectedTrainIndex - 1);
				} else {
					VM.SetSelectedTrain(VM.SelectedTrainIndex + 1);
				}
			}

			/// <summary>
			/// Emits the CanExecuteChanged event.
			/// </summary>
			public void EmitCanExecuteChanged() {
				CanExecuteChanged?.Invoke(this, EventArgs.Empty);
			}

			private readonly TrainDetailViewModel VM;
			private readonly bool Decrement;
		}

		private readonly SwitchTrainCommand PrevCommandImpl, NextCommandImpl;
		private IReadOnlyList<Train> Trains;
		private uint SelectedTrainIndex;

		/// <summary>
		/// Selects a specific train.
		/// </summary>
		/// <param name="selectedTrain">The index of the train to select.</param>
		private void SetSelectedTrain(uint selectedTrain) {
			if (selectedTrain != SelectedTrainIndex) {
				SelectedTrainIndex = selectedTrain;
				EmitPropertyChanged(nameof(SelectedTrain));
			}
		}

		/// <summary>
		/// Emits a property change event.
		/// </summary>
		/// <param name="property">The name of the property that changed.</param>
		private void EmitPropertyChanged(string property) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
		}
	}
}
