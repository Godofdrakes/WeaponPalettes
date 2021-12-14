using System.Collections.Generic;
using JetBrains.Annotations;
using SideLoader.SaveData;

namespace WeaponPalettes
{
	[UsedImplicitly]
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