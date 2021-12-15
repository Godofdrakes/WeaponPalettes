using System;
using System.Collections.Generic;

namespace WeaponPalettes
{
	public class CharacterMap
	{
		private readonly Dictionary<UID, WeaponPaletteMap> _weaponPalettes = new();

		private readonly Dictionary<UID, WeaponSet> _weaponSets = new();

		private bool _bypass;

		private WeaponPaletteMap GetWeaponPalettes(Character character)
		{
			if (!_weaponPalettes.TryGetValue(character.UID, out var weaponPalettes))
			{
				weaponPalettes = _weaponPalettes[character.UID] = new WeaponPaletteMap();
			}

			return weaponPalettes;
		}

		public void WeaponSetChanged(Character character)
		{
			var weaponSet = UpdateWeaponSet(character);

			Plugin.Instance.Logger.LogInfo($"[Change] Character: {character.Name}, MainHand: {weaponSet.MainHand}, OffHand: {weaponSet.OffHand}");

			LoadWeaponPalette(character, weaponSet);
		}

		public void SetQuickSlot(Character character, Item item, int index)
		{
			if (_bypass) return;

			Plugin.Instance.Logger.LogInfo($"[Set] Character: {character.Name}, Item: {item.Name}, Index:{index}");

			var weaponSet = GetWeaponSet(character);
			var weaponPalettes = GetWeaponPalettes(character);
			var weaponPalette = weaponPalettes.GetOrAddWeaponPalette(weaponSet);

			weaponPalette.QuickSlots[index] = item.UID;
		}

		public void ClearQuickSlot(Character character, int index)
		{
			if (_bypass) return;

			Plugin.Instance.Logger.LogInfo($"[Clear] Character: {character.Name}, Index:{index}");

			var weaponSet = GetWeaponSet(character);
			var weaponPalettes = GetWeaponPalettes(character);
			var weaponPalette = weaponPalettes.GetWeaponPalette(weaponSet);

			weaponPalette?.QuickSlots.Remove(index);
		}

		private WeaponSet GetWeaponSet(Character character) => _weaponSets[character.UID];

		private static string GetWeaponId(Weapon weapon)
		{
			return weapon != null ? Plugin.Settings.MatchUid ? weapon.UID : weapon.Type.ToString() : string.Empty;
		}

		private WeaponSet UpdateWeaponSet(Character character)
		{
			var mainHand = string.Empty;
			var offHand = string.Empty;

			if (character.CurrentWeapon != null)
			{
				var weapon = character.CurrentWeapon;

				if (Plugin.Settings.MatchMainHand || (weapon.TwoHanded && Plugin.Settings.MatchOffHand))
				{
					mainHand = GetWeaponId(weapon);
				}
			}

			if (character.LeftHandWeapon != null)
			{
				var weapon = character.LeftHandWeapon;

				if (Plugin.Settings.MatchOffHand)
				{
					offHand = GetWeaponId(weapon);
				}
			}
			else if (character.LeftHandEquipment != null)
			{
				var equipment = character.LeftHandEquipment;

				if (Plugin.Settings.MatchOffHand)
				{
					// Lanterns and stuff
					// @todo: equipment can only be matched by UID
					offHand = equipment.UID;
				}
			}

			return _weaponSets[character.UID] = new WeaponSet(mainHand, offHand);
		}

		private void LoadWeaponPalette(Character character, WeaponSet weaponSet)
		{
			if (character is null)
				throw Util.NullParam(nameof(character));

			var weaponPalettes = GetWeaponPalettes(character);
			if (weaponPalettes is null)
				throw Util.PluginException("weaponPalettes is null");
			
			var quickSlotManager = character.QuickSlotMngr;
			if (quickSlotManager is null)
				throw Util.PluginException("quickSlotManager is null");

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