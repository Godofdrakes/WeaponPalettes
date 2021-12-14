using System.Collections.Generic;
using HarmonyLib;

namespace WeaponPalettes.Patches
{
	[HarmonyPatch(typeof(Character))]
	public static class CharacterPatches
	{
		public static readonly Dictionary<UID, WeaponSet> LastWeaponSet = new();

		public static bool DidWeaponSetChange(Character character, WeaponSet weaponSet)
		{
			return !LastWeaponSet.TryGetValue(character.UID, out var old) || !old.Equals(weaponSet);
		}

		public static void ApplyQuickslots(Character character, WeaponSet weaponSet)
		{
			CharacterQuickSlotManagerPatches.GetWeaponPalette(character, weaponSet)?.ApplyToCharacter(character);
		}
		
		[HarmonyPatch("WeaponChanged")]
		[HarmonyPatch("LeftHandChanged")]
		[HarmonyPostfix]
		public static void WeaponSetChanged(Character __instance)
		{
			if (!__instance.IsLocalPlayer)
				return;
			
			// Plugin.Instance.Logger.LogInfo($"Character: {__instance.Name}");
			// Plugin.Instance.Logger.LogInfo($"Weapon: {(__instance.CurrentWeapon ? __instance.CurrentWeapon.Name : "none")}, Type: {(__instance.CurrentWeapon ? __instance.CurrentWeapon.Type.ToString() : "none")}");
			// Plugin.Instance.Logger.LogInfo($"Offhand: {(__instance.LeftHandWeapon ? __instance.LeftHandWeapon.Name : "none")}, Type: {(__instance.LeftHandWeapon ? __instance.LeftHandWeapon.Type.ToString() : "none")}");
			// Plugin.Instance.Logger.LogInfo($"Equipment: {(__instance.LeftHandEquipment ? __instance.LeftHandEquipment.Name : "none")}");

			var weaponSet = WeaponSet.FromCharacter(__instance);
			if (DidWeaponSetChange(__instance, weaponSet))
			{
				LastWeaponSet[__instance.UID] = weaponSet;
				ApplyQuickslots(__instance, weaponSet);
			}
		}
	}
}