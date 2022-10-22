using System;
using HarmonyLib;

// ReSharper disable InconsistentNaming

namespace WeaponPalettes.Patches
{
	[HarmonyPatch(typeof(QuickSlot))]
	public class QuickSlotPatches
	{
		public static event Action<Character, int, Item?>? QuickSlotChanged; 

		[HarmonyPatch(nameof(QuickSlot.Clear))]
		[HarmonyPostfix]
		public static void ClearQuickSlot(QuickSlot __instance)
		{
			var character = __instance.OwnerCharacter;
			if (!character.IsValidLocalPlayer())
				return;

			QuickSlotChanged?.Invoke(character, __instance.Index, null);
		}
		
		[HarmonyPatch(nameof(QuickSlot.SetQuickSlot))]
		[HarmonyPostfix]
		public static void SetQuickSlot(QuickSlot __instance, Item _item)
		{
			var character = __instance.OwnerCharacter;
			if (!character.IsValidLocalPlayer())
				return;

			QuickSlotChanged?.Invoke(character, __instance.Index, _item);
		}
	}
}