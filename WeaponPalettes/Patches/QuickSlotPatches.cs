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

			Plugin.CharacterMap.ClearQuickSlot(character, __instance.Index);
		}
		
		[HarmonyPatch(nameof(QuickSlot.SetQuickSlot))]
		[HarmonyPostfix]
		public static void SetQuickSlot(QuickSlot __instance, Item _item)
		{
			var character = __instance.OwnerCharacter;

			if (!Util.IsLocalPlayer(character))
				return;

			Plugin.CharacterMap.SetQuickSlot(character, _item, __instance.Index);
		}
	}
}