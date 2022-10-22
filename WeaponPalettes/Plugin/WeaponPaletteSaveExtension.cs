using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using SideLoader.SaveData;
using WeaponPalettes.Model;

namespace WeaponPalettes.Plugin
{

	[UsedImplicitly]
	public class WeaponPaletteSaveExtension : PlayerSaveExtension
	{
		[IgnoreDataMember]
		public static Action<Character, IReadOnlyList<SavedQuickSlot>>? OnLoad;

		[IgnoreDataMember]
		public static Action<Character, Action<IEnumerable<SavedQuickSlot>>>? OnSave;

		[DataMember]
		public List<SavedQuickSlot> Data { get; set; } = new();

		public override void ApplyLoadedSave(Character character, bool isWorldHost)
		{
			OnLoad?.Invoke(character, Data);
		}

		public override void Save(Character character, bool isWorldHost)
		{
			Data.Clear();
			OnSave?.Invoke(character, slots => Data.AddRange(slots));
		}
	}
}