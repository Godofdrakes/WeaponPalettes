using System.Collections.Generic;
using System.Linq;

namespace WeaponPalettes
{
	public class WeaponPalette
	{
		public struct Datum
		{
			public WeaponSet WeaponSet;
			public string ItemUid;
			public int Index;
		}

		public readonly Dictionary<int, string> QuickSlots = new();

		public IEnumerable<Datum> Export(WeaponSet weaponSet)
		{
			return QuickSlots.Select(pair => new Datum {WeaponSet = weaponSet, ItemUid = pair.Value, Index = pair.Key});
		}
	}
}