using System;
using System.Collections.Generic;
using System.Linq;

namespace WeaponPalettes
{
	public class WeaponPaletteMap
	{
		private readonly Dictionary<WeaponSet, WeaponPalette> _weaponPalettes = new();

		public WeaponPalette GetWeaponPalette(WeaponSet weaponSet) =>
			_weaponPalettes.TryGetValue(weaponSet, out var weaponPalette) ? weaponPalette : null;

		public WeaponPalette GetOrAddWeaponPalette(WeaponSet weaponSet)
		{
			if (!_weaponPalettes.TryGetValue(weaponSet, out var weaponPalette))
			{
				_weaponPalettes[weaponSet] = weaponPalette = new WeaponPalette();
			}

			return weaponPalette;
		}

		public IEnumerable<KeyValuePair<int, string>> GetQuickSlots(WeaponSet weaponSet)
		{
			return GetWeaponPalette(weaponSet)?.QuickSlots.AsEnumerable();
		}

		public IEnumerable<WeaponPalette.Datum> Export() =>
			_weaponPalettes.SelectMany(pair => pair.Value.Export(pair.Key));
	}
}