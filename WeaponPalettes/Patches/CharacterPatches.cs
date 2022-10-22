using System;
using HarmonyLib;

// ReSharper disable InconsistentNaming

namespace WeaponPalettes.Patches
{
	[HarmonyPatch(typeof(Character))]
	public static class CharacterPatches
	{
		public static event Action<Character>? WeaponSetChanged;
		public static event Action<Character>? SheatheDone;

		[HarmonyPatch(nameof(Character.WeaponChanged))]
		[HarmonyPatch(nameof(Character.LeftHandChanged))]
		[HarmonyPostfix]
		public static void OnWeaponSetChanged(Character __instance)
		{
			if (!__instance.IsValidLocalPlayer())
				return;

			WeaponSetChanged?.Invoke(__instance);
		}

		[HarmonyPatch(nameof(Character.SheatheDone))]
		[HarmonyPostfix]
		public static void OnSheatheDone(Character __instance)
		{
			if (!__instance.IsValidLocalPlayer())
				return;

			SheatheDone?.Invoke(__instance);
		}
	}
}