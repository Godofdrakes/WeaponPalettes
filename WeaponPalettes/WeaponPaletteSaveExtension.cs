using System.Collections.Generic;
using SideLoader.SaveData;

namespace WeaponPalettes
{
	public class WeaponPaletteSaveExtension : PlayerSaveExtension
	{
		public List<WeaponPalette.Datum> Data = new();

		public override void ApplyLoadedSave(Character character, bool isWorldHost)
		{
			Plugin.CharacterMap.Import(this, character);
		}

		public override void Save(Character character, bool isWorldHost)
		{
			Plugin.CharacterMap.Export(this, character);
		}
	}
}