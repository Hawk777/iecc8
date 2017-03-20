namespace Iecc8.Schema {
	/// <summary>
	/// An entry in a region identifying a sub-area that is contained in the region.
	/// </summary>
	public struct RegionEntry {
		/// <summary>
		/// The ID number of the sub-area.
		/// </summary>
		public ushort ID {
			get;
			set;
		}

		/// <summary>
		/// The name of the sub-area.
		/// </summary>
		/// <remarks>
		/// This must match the filename of a XAML file in the subdirectory of Region that matches the region name.
		/// </remarks>
		public string Name {
			get;
			set;
		}
	}
}
