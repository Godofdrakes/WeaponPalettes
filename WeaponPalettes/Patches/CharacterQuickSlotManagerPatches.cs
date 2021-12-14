using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HarmonyLib;

namespace WeaponPalettes.Patches
{
	[HarmonyPatch(typeof(CharacterQuickSlotManager))]
	public static class CharacterQuickSlotManagerPatches
	{
		public static readonly Dictionary<UID, Dictionary<WeaponSet, WeaponPalette>> CharacterWeaponPalettes = new();

		public static IReadOnlyDictionary<WeaponSet, WeaponPalette> GetCharacterWeaponPalettes(Character character)
		{
			return CharacterWeaponPalettes.TryGetValue(character.UID, out var weaponPalettes)
				? weaponPalettes : null;
		}

		public static WeaponPalette GetWeaponPalette(Character character, WeaponSet weaponSet, bool create = false)
		{
			if (!CharacterWeaponPalettes.TryGetValue(character.UID, out var weaponPalettes))
			{
				if (!create)
					return null;
				
				weaponPalettes = CharacterWeaponPalettes[character.UID] = new Dictionary<WeaponSet, WeaponPalette>();
			}

			if (!weaponPalettes.TryGetValue(weaponSet, out var weaponPalette))
				return create ? weaponPalettes[weaponSet] = new WeaponPalette() : null;

			return weaponPalette;
		}

		public static void SetCharacterWeaponPalette(Character character, IReadOnlyDictionary<WeaponSet, WeaponPalette> weaponPalettes)
		{
			CharacterWeaponPalettes[character.UID] = weaponPalettes.ToDictionary(pair => pair.Key, pair => pair.Value);
		}

		public static void ClearWeaponPaletteQuickSlot(Character character, int index)
		{
			Plugin.Instance.Logger.LogDebug($"[Remove] Character: {character.Name}, Index: {index}");
			
			var weaponSet = WeaponSet.FromCharacter(character);
			var weaponPalette = GetWeaponPalette(character, weaponSet);
			weaponPalette?.ItemUids.Remove(index);
		}

		public static void SetWeaponPaletteQuickSlot(Character character, int index, Item item)
		{
			if (item == null)
			{
				ClearWeaponPaletteQuickSlot(character, index);
				return;
			}

			var weaponSet = WeaponSet.FromCharacter(character);
			var weaponPalette = GetWeaponPalette(character, weaponSet, true);
			weaponPalette.ItemUids[index] = item.UID;
		}

		[HarmonyPatch(nameof(CharacterQuickSlotManager.ClearQuickSlot))]
		[HarmonyPostfix]
		public static void ClearQuickSlot(Character ___m_character, int _index)
		{
			if (!IsLocalCharacter(___m_character))
				return;
			
			ClearWeaponPaletteQuickSlot(___m_character, _index);
		}

		public static bool IsLocalCharacter(Character character) => character != null && character.IsLocalPlayer;

		[HarmonyPatch(nameof(CharacterQuickSlotManager.SetQuickSlot))]
		[HarmonyPostfix]
		public static void SetQuickSlot(Character ___m_character, int _index, Item _item)
		{
			if (!IsLocalCharacter(___m_character))
				return;
			
			SetWeaponPaletteQuickSlot(___m_character, _index, _item);
		}
	}
}