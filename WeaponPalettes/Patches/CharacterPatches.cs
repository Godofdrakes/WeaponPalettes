using System.Collections.Generic;
using HarmonyLib;

namespace WeaponPalettes.Patches
{
	[HarmonyPatch(typeof(Character))]
	public static class CharacterPatches
	{
		[HarmonyPatch("WeaponChanged")]
		[HarmonyPatch("LeftHandChanged")]
		[HarmonyPostfix]
		public static void WeaponSetChanged(Character __instance)
		{
			if (!Util.IsLocalPlayer(__instance))
				return;
			
			Plugin.CharacterMap.WeaponSetChanged(__instance);
		}
	}
}