using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SideLoader.SaveData;
using WeaponPalettes.Model;

namespace WeaponPalettes.Plugin
{
	[UsedImplicitly]
	public class WeaponPaletteSaveExtension : PlayerSaveExtension
	{
		public static Action<Character, IReadOnlyList<SavedQuickSlot>>? OnLoad;
		public static Action<Character, Action<IEnumerable<SavedQuickSlot>>>? OnSave;

		public SavedQuickSlot[]? QuickSlots = Array.Empty<SavedQuickSlot>();

		public override void ApplyLoadedSave(Character character, bool isWorldHost)
		{
			if (OnLoad is null) throw ExHelper.ArgumentNull(nameof(OnLoad));
			if (QuickSlots is null) throw ExHelper.ArgumentNull(nameof(QuickSlots));

			OnLoad(character, QuickSlots);
		}

		public override void Save(Character character, bool isWorldHost)
		{
			if (OnSave is null) throw new NullReferenceException(nameof(OnLoad));

			var list = new List<SavedQuickSlot>();

			OnSave(character, slots => list.AddRange(slots));

			QuickSlots = list.ToArray();
		}
	}
}