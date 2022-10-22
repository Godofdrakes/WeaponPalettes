using System;
using System.Collections.Generic;

namespace WeaponPalettes.Model
{
	public class CharacterMap
	{
		private bool IsLoading { get; set; }

		private Dictionary<UID, WeaponPaletteMap> CharacterWeaponPaletteMaps { get; } = new();

		private Dictionary<UID, WeaponSet> CharacterWeaponSets { get; } = new();

		private bool TryGetPaletteMap(Character character, out WeaponPaletteMap? palette)
		{
			if (character is null) throw ExHelper.ArgumentNull(nameof(character));
			return CharacterWeaponPaletteMaps.TryGetValue(character.UID, out palette);
		}

		private WeaponPaletteMap GetOrAddPaletteMap(Character character)
		{
			if (!TryGetPaletteMap(character, out var weaponPalettes))
			{
				weaponPalettes = new WeaponPaletteMap();
				CharacterWeaponPaletteMaps[character.UID] = weaponPalettes;
			}

			return weaponPalettes!;
		}

		public void SetQuickSlot(Character character, int index, Item item)
		{
			if (IsLoading) return;

			if (!CharacterWeaponSets.TryGetValue(character.UID, out var weaponSet))
			{
				weaponSet = WeaponSet.Empty;
			}

			var paletteMap = GetOrAddPaletteMap(character);
			var weaponPalette = paletteMap.GetOrAddPalette(weaponSet!);
			weaponPalette.SetItem(index, item.UID);
		}

		public void ClearQuickSlot(Character character, int index)
		{
			if (IsLoading) return;

			if (!CharacterWeaponSets.TryGetValue(character.UID, out var weaponSet))
			{
				weaponSet = WeaponSet.Empty;
			}

			var paletteMap = GetOrAddPaletteMap(character);
			var palette = paletteMap.GetOrAddPalette(weaponSet!);
			palette!.RemoveItem(index);
		}

		public void LoadWeaponPalette(Character character, WeaponSet weaponSet)
		{
			if (character is null) throw ExHelper.ArgumentNull(nameof(character));

			var paletteMap = GetOrAddPaletteMap(character);
			var palette = paletteMap.GetOrAddPalette(weaponSet);

			IsLoading = true;
			palette!.Apply(character.QuickSlotMngr);
			IsLoading = false;

			CharacterWeaponSets[character.UID] = weaponSet;
		}

		public void Import(Character character, IEnumerable<SavedQuickSlot> saveData)
		{
			if (character is null) throw ExHelper.ArgumentNull(nameof(character));
			if (saveData is null) throw ExHelper.ArgumentNull(nameof(saveData));

			var paletteMap = GetOrAddPaletteMap(character);

			if (paletteMap is null)
				throw new NullReferenceException(nameof(paletteMap));

			paletteMap.Reset();

			foreach (var savedQuickSlot in saveData)
			{
				paletteMap.GetOrAddPalette(savedQuickSlot.WeaponSet)
					.SetItem(savedQuickSlot.Index, savedQuickSlot.Item);
			}
		}

		public IEnumerable<SavedQuickSlot> Export(Character character)
		{
			var list = new List<SavedQuickSlot>();

			if (TryGetPaletteMap(character, out var paletteMap))
			{
				list.AddRange(paletteMap!.Export());
			}

			return list;
		}

		public void Reset()
		{
			this.IsLoading = false;
			this.CharacterWeaponSets.Clear();
			this.CharacterWeaponPaletteMaps.Clear();
		}
	}
}
