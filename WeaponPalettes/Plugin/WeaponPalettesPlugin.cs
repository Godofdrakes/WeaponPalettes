using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using WeaponPalettes.Model;
using WeaponPalettes.Patches;

namespace WeaponPalettes.Plugin
{
	[BepInPlugin(GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
	[BepInDependency("com.sinai.SideLoader", "3.8.4")]
	public class WeaponPalettesPlugin : BaseUnityPlugin
	{
		private const string GUID = "com.GodofDrakes.WeaponPalettes";

		private PluginSettings Settings { get; set; } = new(true, true, false, true);
		private CharacterMap CharacterMap { get; } = new();
		private Harmony? HarmonyInstance { get; set; }

		private void Awake()
		{
			Settings = PluginSettings.Load(this);
			HarmonyInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
			CharacterMap.Reset();

			Logger.LogDebug($"MatchType: {Settings.MatchType}");
			Logger.LogDebug($"MatchMainHand: {Settings.MatchMainHand}");
			Logger.LogDebug($"MatchOffHand: {Settings.MatchOffHand}");
			Logger.LogDebug($"FakeEmptyHand: {Settings.FakeEmptyHand}");

			QuickSlotPatches.QuickSlotChanged += OnQuickSlotChanged;
			CharacterPatches.WeaponSetChanged += OnWeaponSetChanged;
			CharacterPatches.SheatheDone += OnSheatheInput;
			WeaponPaletteSaveExtension.OnLoad += OnLoad;
			WeaponPaletteSaveExtension.OnSave += OnSave;
		}

		private void OnDestroy()
		{
			HarmonyInstance!.UnpatchSelf();

			QuickSlotPatches.QuickSlotChanged -= OnQuickSlotChanged;
			CharacterPatches.WeaponSetChanged -= OnWeaponSetChanged;
			CharacterPatches.SheatheDone -= OnSheatheInput;
			WeaponPaletteSaveExtension.OnLoad -= OnLoad;
			WeaponPaletteSaveExtension.OnSave -= OnSave;
		}

		private void OnLoad(Character character, IReadOnlyList<SavedQuickSlot> savedQuickSlots)
		{
			Logger.LogInfo($"[OnLoad] Character:{character.Name}");
			Logger.LogInfo($"[OnLoad] Loading {savedQuickSlots.Count} SavedQuickSlot instances");

			foreach (var quickSlot in savedQuickSlots)
			{
				Logger.LogDebug($"[OnLoad] WeaponSet:{quickSlot.WeaponSet}, Item:{quickSlot.ItemId}, Index:{quickSlot.Index}");
			}

			CharacterMap.Import(character, savedQuickSlots);
		}

		private void OnSave(Character character, Action<IEnumerable<SavedQuickSlot>> addSavedQuickSlots)
		{
			Logger.LogInfo($"[OnSave] Character:{character.Name}");

			var savedQuickSlots = CharacterMap.Export(character).ToList();
			Logger.LogInfo($"[OnLoad] Saving {savedQuickSlots.Count} SavedQuickSlot instances");

			foreach (var quickSlot in savedQuickSlots)
			{
				Logger.LogDebug($"[OnLoad] WeaponSet:{quickSlot.WeaponSet}, Item:{quickSlot.ItemId}, Index:{quickSlot.Index}");
			}

			addSavedQuickSlots.Invoke(savedQuickSlots);
		}

		private void OnQuickSlotChanged(Character character, int index, Item? item)
		{
			var itemName = item != null ? item.Name : "<null>";
			var itemUid = item != null ? item.UID : "<invalid>";
			Logger.LogDebug($"[OnQuickSlotChanged] Character:{character.Name}, Index:{index}, Item:{itemName}, UID:{itemUid}");

			if (item is null)
			{
				CharacterMap.ClearQuickSlot(character, index);
			}
			else
			{
				CharacterMap.SetQuickSlot(character, index, item);
			}
		}

		private void OnWeaponSetChanged(Character character)
		{
			var weaponSet = MakeWeaponSet(character, Settings);
			Logger.LogInfo($"[OnQuickSlotChanged] Character:{character.Name}, WeaponSet:{weaponSet}");
			CharacterMap.LoadWeaponPalette(character, weaponSet);
		}

		private void OnSheatheInput(Character character)
		{
			var weaponSet = MakeWeaponSet(character, Settings);
			Logger.LogInfo($"[OnSheatheInput] Character:{character.Name}, WeaponSet:{weaponSet}");
			CharacterMap!.LoadWeaponPalette(character, weaponSet);
		}

		private static string GetWeaponId(Weapon weapon, PluginSettings settings)
		{
			// Don't use UID here as that's unique to each instance of an item
			// Multiple instances of the same item will have different UIDs
			if (weapon == null) return string.Empty;
			return !settings.MatchType ? weapon.Name : weapon.Type.ToString();
		}

		private static WeaponSet MakeWeaponSet(Character character, PluginSettings settings)
		{
			var mainHand = string.Empty;
			var offHand = string.Empty;

			if (settings.MatchMainHand && !(settings.FakeEmptyHand && character.Sheathed))
			{
				if (character.CurrentWeapon != null)
				{
					mainHand = GetWeaponId(character.CurrentWeapon, settings);
				}
			}

			if (settings.MatchOffHand)
			{
				if (character.CurrentWeapon != null && character.CurrentWeapon.TwoHanded)
				{
					offHand = GetWeaponId(character.CurrentWeapon, settings);
				}
				else if (character.LeftHandWeapon != null)
				{
					offHand = GetWeaponId(character.LeftHandWeapon, settings);
				}
				else if (character.LeftHandEquipment != null)
				{
					// Lanterns and stuff
					// @todo: equipment has no type and can only be matched by name
					offHand = character.LeftHandEquipment.Name;
				}
			}

			return new WeaponSet(mainHand, offHand);
		}
	}
}