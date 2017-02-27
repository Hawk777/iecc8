using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Iecc8.Messages {
	/// <summary>
	/// The states of signals.
	/// </summary>
	[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DispatcherComms.MessagesFromRun8")]
	public struct SignalsMessage {
		/// <summary>
		/// Which sub-area the signals are in.
		/// </summary>
		[DataMember]
		public int Route;

		/// <summary>
		/// Which indication has been requested from each signal.
		/// </summary>
		[DataMember]
		public List<ESignalIndication> Signals;

		public override string ToString() {
			int[] counts = new int[Enum.GetValues(typeof(ESignalIndication)).Length];
			foreach (ESignalIndication si in Signals) {
				++counts[(int) si];
			}
			string ret = nameof(SignalsMessage) + "(" + Route + ", " + Signals.Count + " items (";
			bool first = true;
			foreach (ESignalIndication si in Enum.GetValues(typeof(ESignalIndication))) {
				if (!first) {
					ret += " ";
				}
				first = false;
				ret += counts[(int) si];
				ret += " ";
				ret += Enum.GetName(typeof(ESignalIndication), si);
				if (counts[(int) si] < 5) {
					ret += " (";
					bool first2 = true;
					for (int i = 0; i != Signals.Count; ++i) {
						if (Signals[i] == si) {
							if (!first2) {
								ret += ", ";
							}
							first2 = false;
							ret += i;
						}
					}
					ret += ")";
				}
			}
			ret += "))";
			return ret;
		}
	}
}
