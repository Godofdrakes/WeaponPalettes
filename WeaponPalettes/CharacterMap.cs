using System;
using System.Collections.Generic;

namespace WeaponPalettes
{
	public class CharacterMap
	{
		private readonly Dictionary<UID, WeaponPaletteMap> _weaponPalettes = new();

		private readonly Dictionary<UID, WeaponSet> _weaponSets = new();

		private bool _bypass;

		public WeaponPaletteMap GetWeaponPalettes(Character character)
		{
			if (!_weaponPalettes.TryGetValue(character.UID, out var weaponPalettes))
			{
				_weaponPalettes[character.UID] = weaponPalettes = new WeaponPaletteMap();
			}

			return weaponPalettes;
		}

		public void WeaponSetChanged(Character character)
		{
			Plugin.Instance.Logger.LogDebug($"[Change] Character: {character.Name}");

			// Plugin.Instance.Logger.LogInfo($"Weapon: {(__instance.CurrentWeapon ? __instance.CurrentWeapon.Name : "none")}, Type: {(__instance.CurrentWeapon ? __instance.CurrentWeapon.Type.ToString() : "none")}");
			// Plugin.Instance.Logger.LogInfo($"Offhand: {(__instance.LeftHandWeapon ? __instance.LeftHandWeapon.Name : "none")}, Type: {(__instance.LeftHandWeapon ? __instance.LeftHandWeapon.Type.ToString() : "none")}");
			// Plugin.Instance.Logger.LogInfo($"Equipment: {(__instance.LeftHandEquipment ? __instance.LeftHandEquipment.Name : "none")}");

			var weaponSet = _weaponSets[character.UID] = WeaponSet.FromCharacter(character);

			LoadWeaponPalette(character, weaponSet);
		}

		public void SetQuickSlot(Character character, WeaponSet weaponSet, Item item, int index)
		{
			if (_bypass) return;

			Plugin.Instance.Logger.LogDebug($"[Set] Character: {character.Name}, Item: {item.Name}, Index:{index}");

			var weaponPalettes = GetWeaponPalettes(character);
			var weaponPalette = weaponPalettes.GetOrAddWeaponPalette(weaponSet);

			weaponPalette.QuickSlots[index] = item.UID;
		}

		public void ClearQuickSlot(Character character, WeaponSet weaponSet, int index)
		{
			if (_bypass) return;

			Plugin.Instance.Logger.LogDebug($"[Clear] Character: {character.Name}, Index:{index}");

			var weaponPalettes = GetWeaponPalettes(character);
			var weaponPalette = weaponPalettes.GetWeaponPalette(weaponSet);

			weaponPalette?.QuickSlots.Remove(index);
		}

		public WeaponSet GetWeaponSet(Character character) => _weaponSets[character.UID];

		private void LoadWeaponPalette(Character character, WeaponSet weaponSet)
		{
			var weaponPalettes = GetWeaponPalettes(character);
			var quickSlotManager = character.QuickSlotMngr;

			_bypass = true;

			for (var index = 0; index < quickSlotManager.QuickSlotCount; ++index)
			{
				quickSlotManager.ClearQuickSlot(index);
			}

			foreach (var pair in weaponPalettes.GetQuickSlots(weaponSet))
			{
				var item = ItemManager.Instance.GetItem(pair.Value);
				if (item != null)
					quickSlotManager.SetQuickSlot(pair.Key, item);
			}

			quickSlotManager.RefreshQuickSlots();

			_bypass = false;
		}

		public void Import(WeaponPaletteSaveExtension saveExtension, Character character)
		{
			var weaponPalettes = GetWeaponPalettes(character);

			foreach (var datum in saveExtension.Data)
			{
				var weaponPalette = weaponPalettes.GetOrAddWeaponPalette(datum.WeaponSet);

				weaponPalette.QuickSlots[datum.Index] = datum.ItemUid;
			}
		}

		public void Export(WeaponPaletteSaveExtension saveExtension, Character character)
		{
			var weaponPalettes = GetWeaponPalettes(character);
			if (weaponPalettes != null) saveExtension.Data.AddRange(weaponPalettes.Export());
		}
	}
}