using HarmonyLib;

namespace WeaponPalettes.Patches
{
	[HarmonyPatch(typeof(QuickSlot))]
	public class QuickSlotPatches
	{
		[HarmonyPatch(nameof(QuickSlot.Clear))]
		[HarmonyPostfix]
		public static void SetQuickSlot(QuickSlot __instance)
		{
			var character = __instance.OwnerCharacter;

			if (!Util.IsLocalPlayer(character))
				return;

			var weaponSet = Plugin.CharacterMap.GetWeaponSet(character);

			Plugin.CharacterMap.ClearQuickSlot(character, weaponSet, __instance.Index);
		}
		
		[HarmonyPatch(nameof(QuickSlot.SetQuickSlot))]
		[HarmonyPostfix]
		public static void SetQuickSlot(QuickSlot __instance, Item _item)
		{
			var character = __instance.OwnerCharacter;

			if (!Util.IsLocalPlayer(character))
				return;

			var weaponSet = Plugin.CharacterMap.GetWeaponSet(character);

			Plugin.CharacterMap.SetQuickSlot(character, weaponSet, _item, __instance.Index);
		}
	}
}