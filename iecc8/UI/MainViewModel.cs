using System.Diagnostics;

namespace Iecc8.UI {
	public class MainViewModel : BindableBase {
		/// <summary>
		/// The world being interacted with.
		/// </summary>
		public World.World World {
			get {
				return WorldImpl;
			}
		}

		/// <summary>
		/// Constructs a new MainViewModel.
		/// </summary>
		/// <param name="world">The world being interacted with.</param>
		public MainViewModel(World.World world) {
			Debug.Assert(world != null);
			WorldImpl = world;
		}

		private readonly World.World WorldImpl;
	}
}
