using HarmonyLib;

namespace WeaponPalettes.Patches
{
	[HarmonyPatch(typeof(Item))]
	public class ItemPatches
	{
		public const int INDEX_NONE = -1;
		
		[HarmonyPatch(nameof(Item.SetQuickSlot))]
		public static void SetQuickSlot(Item __instance, int __quickSlotIndex)
		{
			var character = __instance.OwnerCharacter;

			if (!Util.IsLocalPlayer(character))
				return;

			Plugin.Instance.Logger.LogDebug(
				$"[Item.SetQuickSlot] Character: {character.Name}, Item: {__instance.Name}, Index:{__instance.QuickSlotIndex}");
		}
	}
}