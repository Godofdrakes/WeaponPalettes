using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.Serialization;
using MonoMod.Utils;
using SideLoader.SaveData;
using WeaponPalettes.Patches;

namespace WeaponPalettes
{
	public class WeaponPaletteSaveExtension : PlayerSaveExtension
	{
		public class Datum
		{
			public WeaponSet WeaponSet;
			public int QuickSlot;
			public string ItemUid;
		}

		public List<Datum> Data = new();

		public override void ApplyLoadedSave(Character character, bool isWorldHost)
		{
			var characterWeaponPalette = new Dictionary<WeaponSet, WeaponPalette>();

			foreach (var datum in Data)
			{
				if (!characterWeaponPalette.TryGetValue(datum.WeaponSet, out var weaponPalette))
					weaponPalette = characterWeaponPalette[datum.WeaponSet] = new WeaponPalette();
				
				weaponPalette.ItemUids.Add(datum.QuickSlot, datum.ItemUid);
			}
			
			CharacterQuickSlotManagerPatches.SetCharacterWeaponPalette(character, characterWeaponPalette);
		}

		public override void Save(Character character, bool isWorldHost)
		{
			foreach (var pair in CharacterQuickSlotManagerPatches.GetCharacterWeaponPalettes(character))
			{
				foreach (var subPair in pair.Value.ItemUids)
				{
					Data.Add(new Datum() { WeaponSet = pair.Key, QuickSlot = subPair.Key, ItemUid = subPair.Value});
				}
			}
		}
	}
}