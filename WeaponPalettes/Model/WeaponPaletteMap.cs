﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace WeaponPalettes.Model
{
	public class WeaponPaletteMap
	{
		private Dictionary<WeaponSet, WeaponPalette> WeaponPalettes { get; } = new();

		public WeaponPaletteMap()
		{
			Reset();
		}

		public bool TryGetPalette(WeaponSet weaponSet, out WeaponPalette? weaponPalette)
		{
			if (weaponSet is null) throw ExHelper.ArgumentNull(nameof(weaponSet));
			return WeaponPalettes.TryGetValue(weaponSet, out weaponPalette);
		}

		public WeaponPalette GetOrAddPalette(WeaponSet weaponSet)
		{
			if (weaponSet is null) throw ExHelper.ArgumentNull(nameof(weaponSet));

			if (!TryGetPalette(weaponSet, out var weaponPalette))
			{
				weaponPalette = new WeaponPalette();
				WeaponPalettes[weaponSet] = weaponPalette;
			}

			return weaponPalette!;
		}

		public void Reset()
		{
			WeaponPalettes.Clear();
			WeaponPalettes[WeaponSet.Empty] = new WeaponPalette();
		}

		public IEnumerable<SavedQuickSlot> Export() => WeaponPalettes.SelectMany(pair => pair.Value.Export()
			.Select(tuple => new SavedQuickSlot
			{
				WeaponSet = pair.Key,
				ItemUid = tuple.itemUid,
				ItemId = tuple.itemId,
				Index = tuple.index
			}));
	}
}